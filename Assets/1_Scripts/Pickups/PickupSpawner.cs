using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour {

    public PickupScript.PickupType currentSpawn = PickupScript.PickupType.Ammo;

    public GameObject machinegunPrefab;
    public GameObject rocketlauncherPrefab;
    public GameObject ammoPrefab;
    public GameObject heatDispersorPrefab;
    public GameObject speedPrefab;
    public GameObject nukePrefab;

    private GameObject _planet;

    void Start ()
	{
        _planet = GameObject.Find("Planet");
	}

	private PickupScript.PickupType GetRandomPickup()
	{
		float r = Random.Range(0f, 100f);

		float delta = (100 - 5) / 3f;

		if(r < delta * 1)
		{
			return PickupScript.PickupType.Ammo;
		}

		if(r < delta * 2)
		{
			return PickupScript.PickupType.HeatDispertion;
		}

		if(r < delta * 3)
		{
			return PickupScript.PickupType.Speed;
		}

		if(r >= 95)
		{
			return PickupScript.PickupType.Nuke;
		}

		return PickupScript.PickupType.None;
	}

    public void SpawnPickup(Vector3 pos, PickupScript.PickupType type = PickupScript.PickupType.None)
    {
        GameObject instance;

		if(type == PickupScript.PickupType.None)
		{
			type = GetRandomPickup();
		}

        switch (type)
        {
            case PickupScript.PickupType.Machinegun:
                instance = Instantiate(machinegunPrefab);
                break;
            case PickupScript.PickupType.Rocketlauncher:
                instance = Instantiate(rocketlauncherPrefab);
                break;
            case PickupScript.PickupType.Ammo:
                instance = Instantiate(ammoPrefab);
                break;
            case PickupScript.PickupType.HeatDispertion:
                instance = Instantiate(heatDispersorPrefab);
                break;
            case PickupScript.PickupType.Speed:
                instance = Instantiate(speedPrefab);
                break;
            case PickupScript.PickupType.Nuke:
                instance = Instantiate(nukePrefab);
                break;
            default:
				return;
        }

        instance.transform.parent = _planet.transform;
        instance.transform.position = pos;
    }
}
