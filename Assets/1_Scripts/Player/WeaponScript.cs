using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace Weapons
{

    public enum WeaponType
    {
        Pistol,
        Machinegun,
        Rocketlauncher
    }

    public class WeaponScript : MonoBehaviour
    {
        const string HEAT_GAUGE = "HeatGauge";
        const string AMMO_GAUGE = "AmmoGauge";

        const float PistolROF = 1;
        const float PistolRange = 200;
        const float PistolDamage = 3;
        const float PistolRadius = 0;
        const float PistolImpactForce = 5000;
        const float PistolProjectileSpeed = 8000;
        //const float PistolHeatLimit = 100;
        const float PistoHeatPerShot = 30;
        const float PistolHeatDissipateRate = 0.5f;

        const float MachinegunROF = 3;
        const float MachinegunRange = 100;
        const float MachinegunDamage = 2;
        const float MachinegunRadius = 0;
        const float MachinegunImpactForce = 10000;
        const float MachinegunProjectileSpeed = 8000;
        //const float MachinegunHeatLimit = 100;
        const float MachinegunHeatPerShot = 20;
        const float MachinegunHeatDissipateRate = 0.5f;

        const float RocketlauncherROF = 0.5f;
        const float RocketlauncherRange = 400;
        const float RocketlauncherDamage = 10;
        const float RocketlauncherRadius = 10;
        const float RocketlauncherImpactForce = 100000;
        const float RocketlauncherProjectileSpeed = 2000;
        const int RocketlauncherAmmoLimit = 10;
        const int RocketlauncherInitialAmmo = 3;


        public WeaponType type;
        public GameObject pistolProjectile, machinegunProjectile, rocketlauncherProjectile;

        private float _rateOfFire; //shots per sec
        private float _range; // so we dont kill the sun by mistake lel
        private float _damage; //self explanatory
        private float _radius; // in case we have rocket launcher or other explosives
        private float _impactForce; // force of the explosion
        private float _projectileSpeed;
        private float _currentHeat;
        private int _currentAmmo;
        private bool _isOnCooldown = false;
        private Image _heatUIGauge, _ammoUIGauge;

        private bool _hasShot = false;

        void Awake()
        {
            SetupGun();
            SetupUI();
            if (type != WeaponType.Rocketlauncher)
                StartCoroutine(DissipateHeat());
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
                case WeaponType.Pistol:
                    _rateOfFire = PistolROF;
                    _range = PistolRange;
                    _damage = PistolDamage;
                    _radius = PistolRadius;
                    _impactForce = PistolImpactForce;
                    _projectileSpeed = PistolProjectileSpeed;
                    _currentHeat = 0f;
                    break;
                case WeaponType.Machinegun:
                    _rateOfFire = MachinegunROF;
                    _range = MachinegunRange;
                    _damage = MachinegunDamage;
                    _radius = MachinegunRadius;
                    _impactForce = MachinegunImpactForce;
                    _projectileSpeed = MachinegunProjectileSpeed;
                    _currentHeat = 0f;
                    break;
                case WeaponType.Rocketlauncher:
                    _rateOfFire = RocketlauncherROF;
                    _range = RocketlauncherRange;
                    _damage = RocketlauncherDamage;
                    _radius = RocketlauncherRadius;
                    _impactForce = RocketlauncherImpactForce;
                    _projectileSpeed = RocketlauncherProjectileSpeed;
                    _currentAmmo = RocketlauncherInitialAmmo;
                    break;
                default:
                    type = WeaponType.Pistol;
                    SetupGun();
                    break;
            }
        }

        void SetupUI()
        {
            _heatUIGauge = GameObject.Find(HEAT_GAUGE).GetComponent<Image>();
            _heatUIGauge.enabled = false;
            _ammoUIGauge = GameObject.Find(AMMO_GAUGE).GetComponent<Image>();
            _ammoUIGauge.enabled = false;
            switch (type)
            {
                case WeaponType.Pistol:
                    _heatUIGauge.enabled = true;
                    _ammoUIGauge.enabled = false;
                    _heatUIGauge.transform.localScale = new Vector3(_heatUIGauge.transform.localScale.x, 0, 1);
                    break;
                case WeaponType.Machinegun:
                    _heatUIGauge.enabled = true;
                    _ammoUIGauge.enabled = false;
                    _heatUIGauge.transform.localScale = new Vector3(_heatUIGauge.transform.localScale.x, 0, 1);
                    break;
                case WeaponType.Rocketlauncher:
                    _heatUIGauge.enabled = false;
                    _ammoUIGauge.enabled = true;
                    _ammoUIGauge.transform.localScale = new Vector3(_ammoUIGauge.transform.localScale.x, (_currentAmmo / (float)RocketlauncherAmmoLimit), 1);
                    break;
                default:
                    break;
            }
        }

        public void Fire()
        {
            if (!_hasShot)
            {
                switch (type)
                {
                    case WeaponType.Pistol:
                        if (_isOnCooldown)
                            return;
                        break;
                    case WeaponType.Machinegun:
                        if (_isOnCooldown)
                            return;
                        break;
                    case WeaponType.Rocketlauncher:
                        if (_currentAmmo == 0)
                            return;
                        break;
                    default:
                        break;
                }
                StartCoroutine(FireRoutine());
            }

        }

        void SpawnProjectile()
        {
            GameObject projectile;
            switch (type)
            {
                case WeaponType.Pistol:
                    projectile = Instantiate(pistolProjectile);
                    break;
                case WeaponType.Machinegun:
                    projectile = Instantiate(machinegunProjectile);
                    break;
                case WeaponType.Rocketlauncher:
                    projectile = (GameObject)Instantiate(rocketlauncherProjectile, Vector3.zero, Quaternion.identity);
                    break;
                default:
                    projectile = Instantiate(pistolProjectile);
                    break;
            }
            //projectile.transform.parent = gameObject.transform;
            projectile.transform.localPosition = transform.position;
            //projectile.transform.localRotation = Quaternion.identity;
            projectile.GetComponent<ProjectileScript>().SetupProjectile(_range, _damage, _radius, _impactForce);
            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * _projectileSpeed, ForceMode.Force);
            projectile.GetComponent<ProjectileScript>().Shoot();
        }

        void UpdateAmmoAndHeatAfterShot()
        {
            switch (type)
            {
                case WeaponType.Pistol:
                    if (_currentHeat < 1)
                        _currentHeat += PistoHeatPerShot / 100f;
                    if (_currentHeat >= 1)
                        _isOnCooldown = true;
                    break;
                case WeaponType.Machinegun:
                    if (_currentHeat < 1)
                        _currentHeat += MachinegunHeatPerShot / 100f;
                    if (_currentHeat >= 1)
                        _isOnCooldown = true;
                    break;
                case WeaponType.Rocketlauncher:
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
                while (waitTime > 0)
                {
                    waitTime -= Time.deltaTime;
                    yield return null;
                }
                _hasShot = false;
            }
        }

        IEnumerator DissipateHeat()
        {
            while (true)
            {
                if (_currentHeat == 0)
                {
                    if (_isOnCooldown)
                        _isOnCooldown = false;
                    yield return new WaitForEndOfFrame();
                }                
                switch (type)
                {
                    case WeaponType.Pistol:
                        if (_currentHeat > 0)
                            _currentHeat -= PistolHeatDissipateRate / 100f;
                        _currentHeat = Mathf.Clamp(_currentHeat, 0, 1);
                        break;
                    case WeaponType.Machinegun:
                        if (_currentHeat > 0)
                            _currentHeat -= MachinegunHeatDissipateRate / 100f;
                        _currentHeat = Mathf.Clamp(_currentHeat, 0, 1);
                        break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public float GetCurrentAmmoOrHeat()
        {
            if (type == WeaponType.Rocketlauncher)
                return (_currentAmmo / (float)RocketlauncherAmmoLimit);
            else return _currentHeat;
        }

        void Update()
        {

        }

        public void ResetAmmoHeat()
        {
            switch (type)
            {
                case WeaponType.Pistol:
                    _currentHeat = 0;
                    break;
                case WeaponType.Machinegun:
                    _currentHeat = 0;
                    break;
                case WeaponType.Rocketlauncher:
                    _currentAmmo = RocketlauncherAmmoLimit;
                    break;
                default:
                    break;
            }
        }
    }
}