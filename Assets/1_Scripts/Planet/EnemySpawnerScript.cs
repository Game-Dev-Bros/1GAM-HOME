using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform landingZoneParent;
    public GameObject landingZonePrefab;

    public float spawnHeight;
    public float descendSpeed;

    private Mesh planetMesh;
    private List<GameObject> _spawnedShips;

	void Awake ()
    {
        planetMesh = GetComponent<MeshFilter>().mesh;
	}

    void Start()
    {
        _spawnedShips = new List<GameObject>();
	}
    
    public void SpawnShip()
    {
        Vector3 surfacePosition = Vector3.zero;
        Vector3 surfaceNormal = Vector3.zero;

        while(true)
        {
            int vertexIndex = Random.Range(0, planetMesh.vertexCount);

            surfacePosition = planetMesh.vertices[vertexIndex];
            surfaceNormal = planetMesh.normals[vertexIndex];

            surfacePosition.x *= transform.localScale.x;
            surfacePosition.y *= transform.localScale.y;
            surfacePosition.z *= transform.localScale.z;

            Collider[] hits = Physics.OverlapSphere(surfacePosition, landingZonePrefab.transform.localScale.x);

            bool canSpawn = true;
            foreach(Collider hit in hits)
            {
                if(hit.transform.tag == "LandingZone")
                {
                    canSpawn = false;
                    break;
                }
            }

            if(canSpawn)
            {
                break;
            }
        }
        
        GameObject landingZone = Instantiate(landingZonePrefab);
        landingZone.transform.position = surfacePosition;
        landingZone.transform.SetParent(landingZoneParent);

        Vector3 spawnPosition = surfacePosition + surfaceNormal * spawnHeight;
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.name = enemyPrefab.name;
        enemy.transform.parent = gameObject.transform;
        enemy.transform.position = spawnPosition;
        enemy.transform.up = surfaceNormal;

        enemy.GetComponent<EnemyShipScript>().AssociateLandingZone(landingZone);
        _spawnedShips.Add(enemy);
    }


    public void ReportShipDestroyed(GameObject s)
    {
        _spawnedShips.Remove(s);
    }

    public void NukeShips()
    {
        var numChildren = gameObject.transform.childCount;
        foreach(var s in _spawnedShips){
            var ship = s.GetComponent<EnemyShipScript>();
            if (ship != null && !ship.IsDead())
                ship.SelfNuke();
        }
        _spawnedShips.Clear();
    }

	public bool IsEmpty()
	{
		return _spawnedShips.Count == 0;
	}
}
