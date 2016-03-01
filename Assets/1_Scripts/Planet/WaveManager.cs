using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class WaveManager : MonoBehaviour
{
	public bool isRunning = true;

	[SerializeField]
	private List<Wave> waveSettings = new List<Wave>();

	private EnemySpawnerScript enemySpawner;
	private HUDScoreScript scoreManager;
    private HUDMessageScript hudMessage;
    private Transform machinegunDrop;
    private Transform rocketlauncherDrop;
    private GameObject _player;
    private PickupSpawner _pickupSpawner;

	private int _waveNumber;
	private int _shipsToSpawn;
	private float _durationInSeconds;
	private float _transitionTimeInSeconds;
	private bool _waveStarted;
	private int _waveShipsSpawned;

    private bool _hasSpawned = false;

	void Awake()
	{
        _player = GameObject.Find(Constants.Game.PLAYER_NAME);
        _pickupSpawner = GameObject.Find("Planet").GetComponent<PickupSpawner>();

		enemySpawner = FindObjectOfType<EnemySpawnerScript>();
		scoreManager = FindObjectOfType<HUDScoreScript>();
		hudMessage = GameObject.Find("HUDMessageCenter").GetComponent<HUDMessageScript>();
	}

	void Start()
	{
        StartCoroutine(SpawnWave());
    }

	void NextWave()
	{
		Wave previous = null;
		Wave current = null;
		Wave next = null;

		_waveNumber++;
		_waveShipsSpawned = 0;

		for(int i = 0; i < waveSettings.Count; i++)
		{
			if(_waveNumber < waveSettings[i].waveNumber)
			{
				previous = waveSettings[i-1];
				next = waveSettings[i];
				break;
			}
			else if(_waveNumber == waveSettings[i].waveNumber)
			{
				current = waveSettings[i];
				break;
			}
		}

		if(current == null)
		{
			if(next != null)
			{
				int waveDifference = next.waveNumber - previous.waveNumber;
				int waveOffset = _waveNumber - previous.waveNumber;

				_shipsToSpawn = previous.shipsToSpawn + ((next.shipsToSpawn - previous.shipsToSpawn) / waveDifference) * waveOffset;
				_durationInSeconds = previous.durationInSeconds + ((next.durationInSeconds - previous.durationInSeconds) / waveDifference) * waveOffset;
				_transitionTimeInSeconds = previous.transitionTimeInSeconds + ((next.transitionTimeInSeconds - previous.transitionTimeInSeconds) / waveDifference) * waveOffset;
			}
			else
			{
				Debug.Log("No more wave settings.");
			}
		}
		else
		{
			_shipsToSpawn = current.shipsToSpawn;
			_durationInSeconds = current.durationInSeconds;
			_transitionTimeInSeconds = current.transitionTimeInSeconds;
		}

		Debug.Log("wave " + _waveNumber);
		Debug.Log("ships: " + _shipsToSpawn);
		Debug.Log("duration: " + _durationInSeconds);
		Debug.Log("transition: " + _transitionTimeInSeconds);
	}

    IEnumerator SpawnWave()
    {
		if(!isRunning)
		{
			yield break;
		}

		DestroyPreviousDebris(5);
		NextWave();
		
		hudMessage.ShowMessage(Constants.Waves.WAVE_START.Replace(Constants.Waves.Tags.WAVE_NUMBER, "" + _waveNumber));

		_waveStarted = false;
		float _currentDuration = _durationInSeconds - _transitionTimeInSeconds;

		while (_currentDuration > 0)
        {
			if(!isRunning)
			{
				yield break;
			}

			_currentDuration -= Time.deltaTime;

            if(!_hasSpawned)
            {
                yield return StartCoroutine(SpawnShip());
            }
			
			if(_waveStarted && _waveShipsSpawned == _shipsToSpawn)
			{
				break;
			}

            yield return null;
        }

		while(!enemySpawner.IsEmpty())
		{
			 yield return null;
		}

		scoreManager.IncreaseScoreBy(Constants.Score.WAVE_CLEARED_MULTIPLIER * _waveNumber);
		hudMessage.ShowMessage(Constants.Waves.WAVE_CLEARED.Replace(Constants.Waves.Tags.WAVE_NUMBER, "" + _waveNumber));

        if (_waveNumber == Constants.Waves.WAVE_MACHINEGUN_DROP)
            _pickupSpawner.SpawnPickup(_player.transform.position + _player.transform.forward * 3, PickupScript.PickupType.Machinegun);
        if(_waveNumber == Constants.Waves.WAVE_MACHINEGUN_DROP)
            _pickupSpawner.SpawnPickup(_player.transform.position + _player.transform.forward * 3, PickupScript.PickupType.Rocketlauncher);

        yield return new WaitForSeconds(_transitionTimeInSeconds);
		yield return StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnShip()
    {
        if (!_hasSpawned)
        {
            _hasSpawned = true;
			_waveStarted = true;

            enemySpawner.SpawnShip();
			_waveShipsSpawned++;

            var waitTime = _durationInSeconds / _shipsToSpawn;
            while (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }

            _hasSpawned = false;
        }
    }

	void DestroyPreviousDebris(float duration = 3)
	{
		EnemyShipDebris[] debris = FindObjectsOfType<EnemyShipDebris>();

		foreach(EnemyShipDebris d in debris) {
			StartCoroutine(d.DestroyNow(duration));
		}
	}
}

[Serializable]
class Wave
{
	public int waveNumber;
	public int shipsToSpawn;
	public float durationInSeconds;
	public float transitionTimeInSeconds;
}