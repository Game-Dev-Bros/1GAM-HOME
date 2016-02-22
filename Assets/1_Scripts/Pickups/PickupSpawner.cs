using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour {

    public GameObject machinegunPrefab;
    public GameObject rocketlauncherPrefab;
    public GameObject ammoPrefab;
    public GameObject heatDispersorPrefab;
    public GameObject speedPrefab;
    public GameObject nukePrefab;

    private GameObject _planet;
    
    // Use this for initialization
    void Start () {
        _planet = GameObject.Find("Planet");
        StartCoroutine(SpawnSequence());
	}

    IEnumerator SpawnSequence()
    {
        while (true)
        {
            var nextWait = Mathf.Round( Random.value * 10 );
            var pos = Random.onUnitSphere/2f;
            SpawnPickup(PickupScript.PickupType.Nuke, pos);
            yield return new WaitForSeconds(nextWait);
        }
    }

    public void SpawnPickup(PickupScript.PickupType type, Vector3 pos)
    {
        GameObject instance;
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
                instance = Instantiate(ammoPrefab);
                break;
        }

        instance.transform.parent = _planet.transform;
        instance.transform.localPosition = pos;
    }
}
