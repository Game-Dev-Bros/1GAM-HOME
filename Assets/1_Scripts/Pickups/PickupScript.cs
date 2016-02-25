using UnityEngine;
using System.Collections;
using Weapons;

public class PickupScript : MonoBehaviour
{

    public enum PickupType
    {
		None,
        Machinegun,
        Rocketlauncher,
        Ammo,
        HeatDispertion,
        Speed,
        Nuke
    }

    public PickupType pickupType = PickupType.Ammo;
    public int ammoPickupAmount = 3;
    public float heatDisperseAmount = 1f;
    public float speedIncrease = 10f;
    public float speedDuration = 10f;
    public float rotationSpeed = 1f;
    public float translationSpeed = 1f;
    public float translationRange = 0.01f;
    public float translationOffset = -0.5f;

    private Transform _item;
    private GameObject _player, _planet;
    private InventoryScript _playerInv;
    private PlayerMovement _playerMovementScript;
    private EnemySpawnerScript _shipSpawner;
    private HUDMessageScript _hud;
    private float _rotationAngle = 0;

    // Use this for initialization
    void Start()
    {
        _hud = GameObject.Find("HUDMessage").GetComponent<HUDMessageScript>();
        _player = GameObject.Find("Rotator");
        _planet = GameObject.Find("Planet");
        _shipSpawner = _planet.GetComponent<EnemySpawnerScript>();
        _playerMovementScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        _playerInv = GameObject.FindWithTag("Player").GetComponent<InventoryScript>();
        _item = transform.GetChild(0);

        transform.forward = (_planet.transform.position - transform.position).normalized;
        //transform.rotation = Quaternion.LookRotation((_player.transform.position - transform.position));

        StartCoroutine(RunAnimation());
    }


    IEnumerator RunAnimation()
    {
        while (true)
        {
            //Debug.DrawRay(transform.position, transform.forward, Color.red, 0.1f);
            _item.localPosition = new Vector3(0, 0, ((Mathf.Cos(Time.time * translationSpeed) * translationRange) + translationOffset));
            _item.localEulerAngles = new Vector3(_item.localEulerAngles.x, _item.localEulerAngles.y, _item.localEulerAngles.z + rotationSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (pickupType)
            {
                case PickupType.Machinegun:
                    _playerInv.hasMachinegun = true;
                    _hud.ShowMessage("Machinegun picked up.\nPress [2] to equip.");
                    Debug.Log("Machinegun picked up");
                    break;
                case PickupType.Rocketlauncher:
                    _playerInv.hasRocketlauncher = true;
                    _hud.ShowMessage("Rocketlauncher picked up.\nPress [3] to equip.");
                    Debug.Log("Rocketlauncher picked up");
                    break;
                case PickupType.Ammo:
                    _playerInv.GiveAmmo(ammoPickupAmount);
                    _hud.ShowMessage("Ammo picked up", 1);
                    Debug.Log("Ammo picked up");
                    break;
                case PickupType.HeatDispertion:
                    _playerInv.RemoveHeat(heatDisperseAmount);
                    _hud.ShowMessage("Heat dispersor picked up", 1);
                    Debug.Log("Heat dispersor picked up");
                    break;
                case PickupType.Speed:
                    _playerMovementScript.GiveBoost(speedDuration, speedIncrease);
                    _hud.ShowMessage("Speed boost picked up", 1);
                    Debug.Log("Speed boost picked up");
                    break;
                case PickupType.Nuke:
                    Debug.Log("Nuke picked up");
                    _hud.ShowMessage("Nuke picked up", 1);
                    _shipSpawner.NukeShips();
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }

    }

}