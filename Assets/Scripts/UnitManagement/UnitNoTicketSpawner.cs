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
        if (unit.CompareTag("Cleaner"))
        {
            spawnCleaner();
        }
        else if (unit.CompareTag("SecurityGuard"))
        {
            spawnSecurityGuard();
        }

        spawnTime -= Time.deltaTime;
    }


    public void spawnCleaner()
    {
        if (spawnTime <= 0 && SubwayStation.main.cleanerCount < 10 &&
            (Mathf.Floor(SubwayStation.main.timeControl.dayTimerNormalized * SubwayStation.main.timeControl.hoursDay) >= 8f))
        {
            SubwayStation.main.cleanerCount++;
            GameObject spawnedUnit = Instantiate(unit, transform.position, transform.rotation);
            OnUnitSpawned?.Invoke(spawnedUnit);

            spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
        }

    }

    public void spawnSecurityGuard()
    {
        if (spawnTime <= 0 && SubwayStation.main.guardCount < 5 &&
            (Mathf.Floor(SubwayStation.main.timeControl.dayTimerNormalized * SubwayStation.main.timeControl.hoursDay) <= 7f))
        {
            SubwayStation.main.guardCount++;
            GameObject spawnedUnit = Instantiate(unit, transform.position, transform.rotation);
            OnUnitSpawned?.Invoke(spawnedUnit);

            spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
        }

    }
}
