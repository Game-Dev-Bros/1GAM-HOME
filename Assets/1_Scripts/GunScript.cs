using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunScript : MonoBehaviour {

    
    public enum GunType
    {
        Pistol,
        Machinegun,
        Rocketlauncher
    }

    const string HEAT_GAUGE = "HeatGauge";
    const string AMMO_GAUGE = "AmmoGauge";

    const float PistolROF = 1;
    const float PistolRange = 200;
    const float PistolDamage = 3;
    const float PistolRadius = 0;
    const float PistolProjectileSpeed = 8000;
    //const float PistolHeatLimit = 100;
    const float PistoHeatPerShot = 50;
    const float PistolHeatDissipateRate = 0.5f;

    const float MachinegunROF = 3;
    const float MachinegunRange = 100;
    const float MachinegunDamage = 2;
    const float MachinegunRadius = 0;
    const float MachinegunProjectileSpeed = 8000;
    //const float MachinegunHeatLimit = 100;
    const float MachinegunHeatPerShot = 20;
    const float MachinegunHeatDissipateRate = 0.5f;

    const float RocketlauncherROF = 0.5f;
    const float RocketlauncherRange = 400;
    const float RocketlauncherDamage = 10;
    const float RocketlauncherRadius = 10;
    const float RocketlauncherProjectileSpeed = 2000;
    const int RocketlauncherAmmoLimit = 10;
    const int RocketlauncherInitialAmmo = 3;


    public GunType type;
    public GameObject pistolProjectile, machinegunProjectile, rocketlauncherProjectile;

    private float _rateOfFire; //shots per sec
    private float _range; // so we dont kill the sun by mistake lel
    private float _damage; //self explanatory
    private float _radius; // in case we have rocket launcher or other explosives
    private float _projectileSpeed;
    private float _currentHeat;
    private int _currentAmmo;
    private bool _isOnCooldown = false, _isReloading = false;
    private Image _heatUIiGauge, _ammoUIGauge;
    private float _gaugeMaxHeat;
    private float _gaugeMaxAmmo;

    private bool _hasShot = false;

	// Use this for initialization
	//void Start () {        
 //       SetupGun();
 //       SetupUI();       
 //   }

    void Awake()
    {
        SetupGun();
        SetupUI();
    }

    void OnValidate()
    {
        SetupGun();
        SetupUI();
    }
    
    void SetupGun()
    {
        switch (type)
        {
            case GunType.Pistol:
                _rateOfFire = PistolROF;
                _range = PistolRange;
                _damage = PistolDamage;
                _radius = PistolRadius;
                _projectileSpeed = PistolProjectileSpeed;
                _currentHeat = 0f;
                break;
            case GunType.Machinegun:
                _rateOfFire = MachinegunROF;
                _range = MachinegunRange;
                _damage = MachinegunDamage;
                _radius = MachinegunRadius;
                _projectileSpeed = MachinegunProjectileSpeed;
                _currentHeat = 0f;
                break;
            case GunType.Rocketlauncher:
                _rateOfFire = RocketlauncherROF;
                _range = RocketlauncherRange;
                _damage = RocketlauncherDamage;
                _radius = RocketlauncherRadius;
                _projectileSpeed = RocketlauncherProjectileSpeed;
                _currentAmmo = RocketlauncherInitialAmmo;
                break;
            default:
                _rateOfFire = PistolROF;
                _range = PistolRange;
                _damage = PistolDamage;
                _radius = PistolRadius;
                _projectileSpeed = PistolProjectileSpeed;
                _currentHeat = 0f;
                break;
        }
    }

    void SetupUI()
    {
        _heatUIiGauge = GameObject.Find(HEAT_GAUGE).GetComponent<Image>();
        _ammoUIGauge = GameObject.Find(AMMO_GAUGE).GetComponent<Image>();
        switch (type)
        {
            case GunType.Pistol:
                _heatUIiGauge.enabled = true;
                _ammoUIGauge.enabled = false;
                _gaugeMaxHeat = _heatUIiGauge.rectTransform.rect.height;
                _heatUIiGauge.transform.localScale = new Vector3(1, 0, 1);
                break;
            case GunType.Machinegun:
                _heatUIiGauge.enabled = true;
                _ammoUIGauge.enabled = false;
                _gaugeMaxHeat = _heatUIiGauge.rectTransform.rect.height;
                _heatUIiGauge.transform.localScale = new Vector3(1, 0, 1);
                break;
            case GunType.Rocketlauncher:
                _heatUIiGauge.enabled = false;
                _ammoUIGauge.enabled = true;
                _gaugeMaxHeat = _ammoUIGauge.rectTransform.rect.height;
                _ammoUIGauge.transform.localScale = new Vector3(1, (_currentAmmo/(float)RocketlauncherAmmoLimit), 1);
                break;
            default:
                break;
        }
    }

    public void Fire()
    {
        if(!_hasShot)
            StartCoroutine(FireRoutine());
    }

    void SpawnProjectile()
    {
        GameObject bullet;
        switch (type)
        {
            case GunType.Pistol:
                bullet = Instantiate(pistolProjectile);
                break;
            case GunType.Machinegun:
                bullet = Instantiate(machinegunProjectile);
                break;
            case GunType.Rocketlauncher:
                bullet = Instantiate(rocketlauncherProjectile);
                break;
            default:
                bullet = Instantiate(pistolProjectile);
                break;
        }
        bullet.transform.parent = gameObject.transform;
        bullet.transform.localPosition = Vector3.zero;
        bullet.GetComponent<ProjectileScript>().SetupProjectile(_range, _damage, _radius);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * _projectileSpeed, ForceMode.Force);
        bullet.GetComponent<ProjectileScript>().Shoot();
    }

    void UpdateAmmoAndHeatAfterShot()
    {
        switch (type)
        {
            case GunType.Pistol:
                if (_currentHeat < 1)
                    _currentHeat += PistoHeatPerShot / 100f;
                if (_currentHeat >= 1)
                    _isOnCooldown = true;
                break;
            case GunType.Machinegun:
                if (_currentHeat < 1)
                    _currentHeat += MachinegunHeatPerShot / 100f;
                if (_currentHeat >= 1)
                    _isOnCooldown = true;
                break;
            case GunType.Rocketlauncher:
                if (_currentAmmo > 0)
                    _currentAmmo--;
                break;
            default:
                break;
        }
    }

    IEnumerator FireRoutine()
    {
        if (!_hasShot)
        {
            _hasShot = true;
            SpawnProjectile();
            UpdateAmmoAndHeatAfterShot();
            var waitTime = 1 / _rateOfFire;
            while(waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }
            _hasShot = false;
        }
    }


    void DissipateHeat()
    {
        if (_currentHeat == 0)
        {
            if (_isOnCooldown)
                _isOnCooldown = false;
            return;
        }
        
        switch (type)
        {
            case GunType.Pistol:
                if(_currentHeat>0)
                    _currentHeat -= PistolHeatDissipateRate/100f;
                _currentHeat = Mathf.Clamp(_currentHeat, 0, 1);
                break;
            case GunType.Machinegun:
                if(_currentHeat>0)
                    _currentHeat -= MachinegunHeatDissipateRate/100f;
                _currentHeat = Mathf.Clamp(_currentHeat, 0, 1);
                break;
        }
    }

    void UpdateUIGauge()
    {
        switch (type)
        {
            case GunType.Pistol:
                _heatUIiGauge.transform.localScale = new Vector3(1,_currentHeat,1);
                break;
            case GunType.Machinegun:
                _heatUIiGauge.transform.localScale = new Vector3(1,_currentHeat,1);
                break;
            case GunType.Rocketlauncher:
                _ammoUIGauge.transform.localScale = new Vector3(1,(_currentAmmo/(float)RocketlauncherAmmoLimit),1);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButton(0))
        {
            switch (type)
            {
                case GunType.Pistol:
                    if (!_isOnCooldown)
                        Fire();
                    break;
                case GunType.Machinegun:
                    if (!_isOnCooldown)
                        Fire();
                    break;
                case GunType.Rocketlauncher:
                    if (_currentAmmo > 0)
                        Fire();
                    break;
                default:
                    break;
            }
        }
        if (Input.GetButtonDown("Reload"))
            ResetAmmoHeat();
        if (Input.GetButtonDown("Drop"))
            DropWeapon();

        //handle heat dissipation
        if (type == GunType.Pistol || type == GunType.Machinegun)
            DissipateHeat();

        UpdateUIGauge();


    }


    void ResetAmmoHeat()
    {
        switch (type)
        {
            case GunType.Pistol:
                _currentHeat = 0;
                break;
            case GunType.Machinegun:
                _currentHeat = 0;
                break;
            case GunType.Rocketlauncher:
                _currentAmmo = RocketlauncherAmmoLimit;
                break;
            default:
                break;
        }
    }

    void DropWeapon()
    {

        transform.parent = null;
    }
}
