﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RingRadarScript : MonoBehaviour {


    public GameObject enemyIndicatorPrefab;

    private List<GameObject> _shipsToTrack;
    private List<GameObject> _indicatorList;
    private GameObject _player;
    private GameObject _radarPlane;
    private EnemySpawnerScript _spawner;
    private EnemyShipScript _currentShipScript;
    private Image _radarCircle;
    private float _enemySpawnDistance;

	// Use this for initialization
	void Awake() {
        _player = GameObject.Find("Rotator");
        _radarPlane = GameObject.Find("RadarPlane");
        //_radarCircle = transform.FindChild("Circle").GetComponentInChildren<Image>();
        _shipsToTrack = new List<GameObject>();
        _indicatorList = new List<GameObject>();
        _spawner = GameObject.Find("Planet").GetComponent<EnemySpawnerScript>();
        _enemySpawnDistance = _spawner.spawnHeight;
        StartCoroutine(TrackPointDistance());
	}

    IEnumerator TrackPointDistance()
    {
        while (true)
        {            
            for(int i = 0; i<_shipsToTrack.Count; i++)
            {
                var currShip = _shipsToTrack[i];
                if(currShip != null)
                    UpdateIndicator(_shipsToTrack[i], i);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void UpdateIndicator(GameObject ship, int index)
    {
        RaycastHit hit;
        var indicator = _indicatorList[index];
        var rayCastDirUp = _player.transform.up.normalized; //_radarPlane.mesh.normals[0];
        //Debug.DrawRay(ship.transform.position, rayCastDirUp * 50, Color.red, 1f);
        if (Physics.Raycast(ship.transform.position, rayCastDirUp, out hit, 500f, LayerMask.GetMask("Radar"))){
            //Debug.Log("It's a hit!");
            var dir = (hit.point - _player.transform.position).normalized;
            //Debug.DrawRay(ship.transform.position, dir * 50, Color.green, 1f);
            indicator.transform.rotation = Quaternion.LookRotation(dir);
            indicator.gameObject.SetActive(true);
        }
        else {
            indicator.gameObject.SetActive(false);
        }

        if (!ship.GetComponent<EnemyShipScript>().hasLanded){
            var pointer = indicator.transform.GetChild(0);
            _currentShipScript = ship.GetComponent<EnemyShipScript>();
            var startLandingHeight = _currentShipScript.GetStartLandingHeight();
            var currDistFromPlanet = _currentShipScript.GetDistanceFromPlanetSurface();
            //pointer.transform.localScale = new Vector3(pointer.transform.localScale.x, pointer.transform.localScale.y, ship.GetComponent<EnemyShipScript>().GetDistanceFromPlanetSurface() / _enemySpawnDistance);
            if(currDistFromPlanet > startLandingHeight)
                pointer.transform.localScale = new Vector3(pointer.transform.localScale.x, pointer.transform.localScale.y, 1);
            else
                pointer.transform.localScale = new Vector3(pointer.transform.localScale.x, pointer.transform.localScale.y, currDistFromPlanet/startLandingHeight);
        }
        else {
            RemoveShipToTrack(ship);
        }
    }

    void CreateIndicator()
    {
        var newInd = Instantiate(enemyIndicatorPrefab);
        newInd.transform.parent = transform;
        newInd.transform.localPosition = new Vector3(0,0,0);
        newInd.transform.localRotation = Quaternion.identity;
        _indicatorList.Add(newInd);
    }
    
    public void RegisterNewShipToTrack(GameObject newShip)
    {
        _shipsToTrack.Add(newShip);
        CreateIndicator();
    }

    public void RemoveShipToTrack(GameObject ship)
    {
        if (_shipsToTrack.Contains(ship))
        {
            var indicator = _indicatorList[_shipsToTrack.IndexOf(ship)];
            _shipsToTrack.Remove(ship);
            _indicatorList.Remove(indicator);
            Destroy(indicator);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
