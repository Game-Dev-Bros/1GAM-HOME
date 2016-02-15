using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyShipScript : MonoBehaviour
{
    public float speed = 10;
    public int hp = 10;

    private bool _hasDied = false;
    private GameObject _planet;
    private bool _landed = false;

    private GameObject _landingZone;

    private List<GameObject> shipDebrisPrefabs = new List<GameObject>();

	void Awake ()
    {
        _planet = GameObject.FindGameObjectWithTag("Planet");
	}

    void Start()
    {
        StartCoroutine(LandShip());

        GameObject debrisParent = GameObject.Find("Debris");
        foreach(Transform child in debrisParent.transform)
        {
            if(child.name == transform.name)
            {
                shipDebrisPrefabs.Add(child.gameObject);
            }
        }
    }

    IEnumerator LandShip()
    {
        while (true)
        {
            if (!_landed && _planet != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _planet.transform.position, speed * Time.deltaTime);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Planet":
                _landed = true;
                break;

            case "PlayerProjectile":
                var ps = other.gameObject.GetComponent<ProjectileScript>();
                hp -= (int)ps.GetProjectileDamage();

                if(hp <= 0 && !_hasDied)
                {
                    _hasDied = true;

                    Vector3 collisionPoint = other.contacts[0].point;
                    float impactForce = ps.GetProjectileImpactForce();

                    SpawnDebris(collisionPoint, impactForce);
                }

                Destroy(other.gameObject);
                break;

            default:
                break;
        }
    }

    void SpawnDebris(Vector3 collisionPoint, float impactForce)
    {
        Destroy(gameObject);
        Destroy(_landingZone);

        if (shipDebrisPrefabs.Count > 0)
        {
            GameObject debris = Instantiate(shipDebrisPrefabs[Random.Range(0, shipDebrisPrefabs.Count)]);

            debris.name = shipDebrisPrefabs[0].name + "Debris";
            debris.SetActive(true);
            debris.transform.SetParent(transform.parent);
            debris.transform.localPosition = transform.localPosition;
            debris.transform.localRotation = transform.localRotation;

            foreach(Transform child in debris.transform)
            {
                child.GetComponent<EnemyShipDebris>().AddExplosionForce(impactForce, collisionPoint, 0.1f);
            }
        }
    }

    public void AssociateLandingZone(GameObject landingZone)
    {
        _landingZone = landingZone;
    }
}
