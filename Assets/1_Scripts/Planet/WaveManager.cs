using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	public bool isRunning = true;

	[SerializeField]
	private List<Wave> waveSettings = new List<Wave>();

	private EnemySpawnerScript enemySpawner;

	private int _waveNumber;
	private int _shipsToSpawn;
	private float _durationInSeconds;
	private float _transitionTimeInSeconds;
	private bool _waveStarted;

    private bool _hasSpawned = false;

	void Awake()
	{
		enemySpawner = FindObjectOfType<EnemySpawnerScript>();
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

		NextWave();

		_waveStarted = false;
		float _currentDuration = _durationInSeconds;

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
			
			if(_waveStarted && enemySpawner.IsEmpty())
			{
				break;
			}

            yield return null;
        }

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

            var waitTime = _durationInSeconds / _shipsToSpawn;
            while (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }

            _hasSpawned = false;
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