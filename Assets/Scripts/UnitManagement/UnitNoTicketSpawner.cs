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
    public ClockTimer controlTime;

    private void Start()
    {
        spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
    }

    private void Update()
    {
        if (spawnTime <= 0 && SubwayStation.main.cleanerCount < 10 && ( (Mathf.Floor(controlTime.dayTimerNormalized * controlTime.hoursDay) <= 2f) || Mathf.Floor(controlTime.dayTimerNormalized * controlTime.hoursDay) >= 8f))
        {
            SubwayStation.main.cleanerCount++;
            GameObject spawnedUnit = Instantiate(unit, transform.position, Quaternion.identity);
            OnUnitSpawned?.Invoke(spawnedUnit);

            spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
        }

        spawnTime -= Time.deltaTime;
    }
}
