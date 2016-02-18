using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyShipScript : MonoBehaviour
{
    public float flyingSpeed = 10;
    public int hp = 10;
    public bool hasLanded = false;

    public float startLandingHeight = 1;
    public float landingSpeed = 5;
    public float postLandingWaitTime = 2;
    private bool _isLanding = false;

    private bool _hasDied = false;
    private GameObject _planet;
    private RingRadarScript _radar;

    private GameObject _landingZone;

    private List<GameObject> shipDebrisPrefabs = new List<GameObject>();

	void Awake ()
    {
        _planet = GameObject.FindGameObjectWithTag("Planet");
        _radar = GameObject.Find("RingRadar").GetComponent<RingRadarScript>();
    }

    void Start()
    {
        _radar.RegisterNewShipToTrack(gameObject);
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
            if (!hasLanded && _planet != null)
            {
                if(!_isLanding)
                {
                    RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.up, startLandingHeight);
                    foreach(RaycastHit hit in hits)
                    {
                        if(hit.transform.gameObject == _planet)
                        {
                            _isLanding = true;
                        }
                    }

                    transform.position += -transform.up * flyingSpeed * Time.deltaTime;
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, _landingZone.transform.position, Time.deltaTime / landingSpeed);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Planet":
                hasLanded = true;
                StartCoroutine(AlmostLanding());
                break;

            case "PlayerProjectile":
                var ps = other.gameObject.GetComponent<ProjectileScript>();
                hp -= (int)ps.GetProjectileDamage();

                if(hp <= 0 && !_hasDied)
                {
                    _hasDied = true;
                    _radar.RemoveShipToTrack(gameObject);
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

    public float GetDistanceFromPlanetSurface()
    {
        RaycastHit hit;
        Physics.Linecast(transform.position, _planet.transform.position, out hit);
        return (hit.point - transform.position).magnitude;
    }

    public void AssociateLandingZone(GameObject landingZone)
    {
        _landingZone = landingZone;
    }

    IEnumerator AlmostLanding()
    {
        Debug.Log("almost landing!");
        yield return new WaitForSeconds(postLandingWaitTime);
        Debug.Log("ship landed - game over!");
    }
}
