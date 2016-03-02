using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Weapons
{
    public class InventoryScript : MonoBehaviour
    {
        const string HEAT_GAUGE = "HeatGauge";
        const string AMMO_GAUGE = "AmmoGauge";

        public bool hasMachinegun = true, hasRocketlauncher = true;
        public GameObject pistolPrefab, machinegunPrefab, rocketlauncherPrefab;

        private WeaponType _equipedWeaponType;
        private WeaponScript pistolScript, machinegunScript, rocketlauncherScript;
        private MeshRenderer pistolMesh, machinegunMesh, rocketlauncherMesh;
        private GameObject weaponEquipPosition;
        private Image _ammoUIGauge, _heatUIGauge;

		private PauseManager pauseManager;
        // Use this for initialization
        void Awake()
        {
            //TODO load player prefs
            weaponEquipPosition = GameObject.Find("WeaponPosition");
			pauseManager = FindObjectOfType<PauseManager>();
            InitWeapons();
            InitUI();
            EquipPistol();

            StartCoroutine(UpdateUIGauge());
        }

        void InitUI()
        {
            _heatUIGauge = GameObject.Find(HEAT_GAUGE).GetComponent<Image>();
            _ammoUIGauge = GameObject.Find(AMMO_GAUGE).GetComponent<Image>();
        }

        void InitWeapons()
        {
            pistolScript = pistolPrefab.GetComponent<WeaponScript>();
            pistolMesh = pistolPrefab.GetComponent<MeshRenderer>();

            machinegunScript = machinegunPrefab.GetComponent<WeaponScript>();
            machinegunMesh = machinegunPrefab.GetComponent<MeshRenderer>();

            rocketlauncherScript = rocketlauncherPrefab.GetComponent<WeaponScript>();
            rocketlauncherMesh = rocketlauncherPrefab.GetComponent<MeshRenderer>();
        }

        IEnumerator UpdateUIGauge()
        {
            while (true)
            {
                switch (_equipedWeaponType)
                {
                    case WeaponType.Pistol:
                        _heatUIGauge.transform.localScale = new Vector3(_heatUIGauge.transform.localScale.x, pistolScript.GetCurrentAmmoOrHeat(), 1);
                        break;
                    case WeaponType.Machinegun:
                        _heatUIGauge.transform.localScale = new Vector3(_heatUIGauge.transform.localScale.x, machinegunScript.GetCurrentAmmoOrHeat(), 1);
                        break;
                    case WeaponType.Rocketlauncher:
                        _ammoUIGauge.transform.localScale = new Vector3(_ammoUIGauge.transform.localScale.x, rocketlauncherScript.GetCurrentAmmoOrHeat(), 1);
                        break;
                    default:
                        break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
        }

        public void GiveAmmo(int amount)
        {
            rocketlauncherScript.IncreaseAmmo(amount);
        }

        public void RemoveHeat(float amount)
        {
            pistolScript.RemoveHeat(amount);
            machinegunScript.RemoveHeat(amount);
        }
        
        void HandleInput()
        {
			if(pauseManager.paused)
			{
				return;
			}

            if (Input.GetButtonDown("EquipPistol"))
                EquipPistol();
            else if (Input.GetButtonDown("EquipMachinegun") && hasMachinegun)
                EquipMachinegun();
            else if (Input.GetButtonDown("EquipRocketlauncher") && hasRocketlauncher)
                EquipRocketlauncher();
            //else if (Input.GetButtonDown("Reload"))
            //{
            //    switch (_equipedWeaponType)
            //    {
            //        case WeaponType.Pistol:
            //            pistolScript.ResetAmmoHeat();
            //            break;
            //        case WeaponType.Machinegun:
            //            machinegunScript.ResetAmmoHeat();
            //            break;
            //        case WeaponType.Rocketlauncher:
            //            rocketlauncherScript.ResetAmmoHeat();
            //            break;
            //        default:
            //            break;
            //    }
            //}
            else if (Input.GetMouseButton(0))
            {
                switch (_equipedWeaponType)
                {
                    case WeaponType.Pistol:
                        pistolScript.Fire();
                        break;
                    case WeaponType.Machinegun:
                        machinegunScript.Fire();
                        break;
                    case WeaponType.Rocketlauncher:
                        rocketlauncherScript.Fire();
                        break;
                    default:
                        break;
                }
            }
        }

        void EquipPistol()
        {
            _equipedWeaponType = WeaponType.Pistol;
            pistolMesh.enabled = true;
            machinegunMesh.enabled = false;
            rocketlauncherMesh.enabled = false;
            _heatUIGauge.enabled = true;
            _ammoUIGauge.enabled = false;
        }

        void EquipMachinegun()
        {
            _equipedWeaponType = WeaponType.Machinegun;
            pistolMesh.enabled = false;
            machinegunMesh.enabled = true;
            rocketlauncherMesh.enabled = false;
            _heatUIGauge.enabled = true;
            _ammoUIGauge.enabled = false;
        }

        void EquipRocketlauncher()
        {
            _equipedWeaponType = WeaponType.Rocketlauncher;
            pistolMesh.enabled = false;
            machinegunMesh.enabled = false;
            rocketlauncherMesh.enabled = true;
            _heatUIGauge.enabled = false;
            _ammoUIGauge.enabled = true;
        }

        public WeaponType GetEquipedWeaponType() { return _equipedWeaponType; }
    }
}
