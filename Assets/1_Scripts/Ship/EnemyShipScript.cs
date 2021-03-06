﻿using UnityEngine;
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
    private GameObject _player;
    private AudioSource _hitConfirm;
    private RingRadarScript _radar;
    private EnemySpawnerScript _spawner;
    private GameObject _landingZone;

	private HUDScoreScript scoreManager;

	private List<GameObject> shipDebrisPrefabs = new List<GameObject>();

	void Awake ()
    {
        _player = GameObject.FindWithTag("Player");
        _hitConfirm = _player.GetComponents<AudioSource>()[1];
        _planet = GameObject.FindGameObjectWithTag("Planet");
        _radar = GameObject.Find("RingRadar").GetComponent<RingRadarScript>();
        _spawner = _planet.GetComponent<EnemySpawnerScript>();

		scoreManager = FindObjectOfType<HUDScoreScript>();
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

                    _spawner.ReportShipDestroyed(gameObject);

					scoreManager.IncreaseScoreBy(Constants.Score.ENEMY_SHIP_KILLED);
                    SpawnDebris(collisionPoint, impactForce);
					FindObjectOfType<PickupSpawner>().SpawnPickup(transform.position);
                }
                _hitConfirm.Play();
                Destroy(other.gameObject);
                break;

            default:
                break;
        }
    }


    private int _implosionForce = 200000; 

    public void SelfNuke()
    {
        _hasDied = true;
        _radar.RemoveShipToTrack(gameObject);
        SpawnDebris(transform.position, _implosionForce);
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

    public float GetStartLandingHeight()
    {
        return startLandingHeight;
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
        yield return new WaitForSeconds(postLandingWaitTime);
		FindObjectOfType<PauseManager>().SetEnded(true);
    }

    public bool IsDead()
    {
        return _hasDied;
    }
}
