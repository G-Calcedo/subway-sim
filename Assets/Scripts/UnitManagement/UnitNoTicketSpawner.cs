using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitNoTicketSpawner : MonoBehaviour
{
    public GameObject unit;

    public float minSpawnRate, maxSpawnRate;
    private float spawnTime = 0;

    public Action<GameObject> OnUnitSpawned;

    private void Start()
    {
        spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
    }

    private void Update()
    {
        if (spawnTime <= 0 && SubwayStation.main.cleanerCount < 10)
        {
            SubwayStation.main.cleanerCount++;
            GameObject spawnedUnit = Instantiate(unit, transform.position, Quaternion.identity);
            OnUnitSpawned?.Invoke(spawnedUnit);

            spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
        }

        spawnTime -= Time.deltaTime;
    }
}
