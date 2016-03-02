using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour
{
    private float _range;
    private float _damage;
    private float _radius;
    private float _impactForce;
    private float _speed;
    private GameObject _player;
    private AudioSource _audioSource;

    private float _currentDistance;

    private float _height;

    // Use this for initialization
    void Awake ()
    {
        _player = GameObject.FindWithTag("Player");
        if (!GetComponent<AudioSource>().Equals(null))
            _audioSource = GetComponent<AudioSource>();

    }
	
    public void SetupProjectile(/*GunScript.GunType t,*/ float ran, float dmg, float rad, float speed, float imp)
    {
        //ChangeBulletType(t);
        _range = ran;
        _damage = dmg;
        _radius = rad;
        _speed = speed;
        _impactForce = imp;
    }

    public void Shoot()
    {
        StartCoroutine(CheckStatus());
    }

    public float GetProjectileDamage()
    {
        return _damage;
    }
    
    public float GetProjectileImpactForce()
    {
        return _impactForce;
    }

    void Update()
    {
        AlignToSurface();
        MoveProjectile();
    }

    void AlignToSurface()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        var hits = Physics.RaycastAll(ray, 2);

        foreach(RaycastHit hit in hits)
        {
            if(hit.transform.tag == "Planet")
            {
                if(_height == 0)
                {
                    _height = (transform.position - hit.point).magnitude;
                }

                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

                transform.position = hit.point + hit.normal * _height;
                transform.rotation = targetRotation;

                break;
            }
        }
    }

    void MoveProjectile()
    {
        Vector3 positionDelta = transform.forward * _speed * Time.deltaTime;
        transform.position += positionDelta;
        _currentDistance += positionDelta.magnitude;
    }

    IEnumerator CheckStatus()
    {

        while(_currentDistance < _range)
        {
            if (_audioSource)
                _audioSource.volume = 1 - (_currentDistance / _range) * 2;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}
