﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int waveNumber = 0;
    public float[] waveFrequency = new float[5] { 0.2f, 0.4f, 0.6f, 1.0f, 1.2f };
    public float[] waveDuration = new float[5] { 25f, 20f, 16f, 13f, 10f };
    public float spawnHeight;
    public float descendSpeed;

    private bool _hasSpawned = false;
    private Mesh planetMesh;

	void Awake ()
    {
        planetMesh = GetComponent<MeshFilter>().mesh;
	}

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        var duration = waveDuration[waveNumber];
        while (duration > 0)
        {
            duration -= Time.deltaTime;

            if(!_hasSpawned)
            {
                StartCoroutine(SpawnOne());
            }

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
        int vertexIndex = Random.Range(0, planetMesh.vertexCount);

        Vector3 surfacePosition = planetMesh.vertices[vertexIndex];
        Vector3 surfaceNormal = planetMesh.normals[vertexIndex];

        surfacePosition.x *= transform.localScale.x;
        surfacePosition.y *= transform.localScale.y;
        surfacePosition.z *= transform.localScale.z;

        Vector3 spawnPosition = surfacePosition + surfaceNormal * spawnHeight;

        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.parent = gameObject.transform;
        enemy.transform.position = spawnPosition;
        enemy.transform.up = surfaceNormal;
    }
}