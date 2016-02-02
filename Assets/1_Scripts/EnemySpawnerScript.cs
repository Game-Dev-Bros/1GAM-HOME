using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour {

    
    public GameObject enemyPrefab;
    public int waveNumber = 0;
    public float[] waveFrequency = new float[5] {0.2f,0.4f, 0.6f, 1f,1.2f};
    public float[] waveDuration = new float[5] { 25f, 20f, 16f, 13f, 10f };

    private bool _hasSpawned = false;
    private GameObject planet;

	// Use this for initialization
	void Awake () {
        planet = GameObject.Find("Planet");
        StartCoroutine(SpawnRoutine());
	}
	

    IEnumerator SpawnRoutine()
    {
        var duration = waveDuration[waveNumber];
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            if(!_hasSpawned)
                StartCoroutine(SpawnOne());
            yield return null;
        }
    }

    IEnumerator SpawnOne()
    {
        if (!_hasSpawned)
        {
            _hasSpawned = true;
            SpawnShip();
            var waitTime = 1 / waveFrequency[waveNumber];
            while (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }
            _hasSpawned = false;
        }
    }
    
    void SpawnShip()
    {
        var randomLoc = Random.onUnitSphere * planet.transform.localScale.x * 2f;
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.parent = gameObject.transform;
        enemy.transform.localPosition = randomLoc;
    }


}
