using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unit;
    public GameObject[] unitModels;

    public float minSpawnRate, maxSpawnRate;
    private float spawnTime = 0;

    public Action<GameObject> OnUnitSpawned;

    private void Update()
    {
        if (spawnTime <= 0)
        {
            GameObject spawnedUnit = Instantiate(unit, transform.position, Quaternion.identity);
            GameObject model = Instantiate(unitModels[UnityEngine.Random.Range(0, unitModels.Length)], spawnedUnit.transform);
            model.transform.localScale *= 2;
            //model.transform.parent = spawnedUnit.transform;
            OnUnitSpawned?.Invoke(spawnedUnit);

            spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
        }

        spawnTime -= Time.deltaTime;
    }
}
