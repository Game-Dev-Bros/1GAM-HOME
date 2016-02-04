using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    private float _range;
    private float _damage;
    private float _radius;

    // Use this for initialization
    void Awake () {
	}
	
    public void SetupProjectile(/*GunScript.GunType t,*/ float ran, float dmg, float rad)
    {
        //ChangeBulletType(t);
        _range = ran;
        _damage = dmg;
        _radius = rad;
    }

    public void Shoot()
    {
        StartCoroutine(CheckStatus());
    }

    public float GetProjectileDamage()
    {
        return _damage;
    }

    IEnumerator CheckStatus()
    {        
        while(Vector3.Magnitude(transform.position-transform.parent.position) < _range)
        {
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }

}
