using UnityEngine;
using System.Collections;



public class GunScript : MonoBehaviour {

    
    public enum GunType
    {
        Pistol,
        Machinegun,
        Rocketlauncher
    }


    const float PistolROF = 1;
    const float PistolRange = 200;
    const float PistolDamage = 3;
    const float PistolRadius = 0;
    const float PistolProjectileSpeed = 2000;

    const float MachinegunROF = 3;
    const float MachinegunRange = 100;
    const float MachinegunDamage = 2;
    const float MachinegunRadius = 0;
    const float MachinegunProjectileSpeed = 2000;

    const float RocketlauncherROF = 0.25f;
    const float RocketlauncherRange = 400;
    const float RocketlauncherDamage = 10;
    const float RocketlauncherRadius = 10;
    const float RocketlauncherProjectileSpeed = 1000;


    public GunType type;
    public GameObject projectile;

    private float _rateOfFire; //shots per sec
    private float _range; // so we dont kill the sun by mistake lel
    private float _damage; //self explanatory
    private float _radius; // in case we have rocket launcher or other explosives
    private float _projectileSpeed;

    private bool _hasShot = false;

	// Use this for initialization
	void Awake () {
        switch (type)
        {
            case GunType.Pistol:
                _rateOfFire = PistolROF;
                _range = PistolRange;
                _damage = PistolDamage;
                _radius = PistolRadius;
                _projectileSpeed = PistolProjectileSpeed;
                break;
            case GunType.Machinegun:
                _rateOfFire = MachinegunROF;
                _range = MachinegunRange;
                _damage = MachinegunDamage;
                _radius = MachinegunRadius;
                _projectileSpeed = MachinegunProjectileSpeed;
                break;
            case GunType.Rocketlauncher:
                _rateOfFire = RocketlauncherROF;
                _range = RocketlauncherRange;
                _damage = RocketlauncherDamage;
                _radius = RocketlauncherRadius;
                _projectileSpeed = RocketlauncherProjectileSpeed;
                break;
            default:
                _rateOfFire = PistolROF;
                _range = PistolRange;
                _damage = PistolDamage;
                _radius = PistolRadius;
                _projectileSpeed = PistolProjectileSpeed;
                break;
        }
        
    }

    public void Fire()
    {
        if(!_hasShot)
            StartCoroutine(FireRoutine());
    }


    IEnumerator FireRoutine()
    {
        if (!_hasShot)
        {
            _hasShot = true;
            var bullet = Instantiate(projectile);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.GetComponent<ProjectileScript>().SetParent(gameObject);
            bullet.GetComponent<ProjectileScript>().SetRange(_range);
            var rb = bullet.GetComponent<Rigidbody>();            
            rb.AddForce(transform.forward*_projectileSpeed, ForceMode.Force);
            bullet.GetComponent<ProjectileScript>().Shoot();

            var waitTime = 1 / _rateOfFire;
            while(waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }
            _hasShot = false;
        }
    }
        	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
            Fire();

	}
}
