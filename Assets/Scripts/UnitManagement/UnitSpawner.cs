using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unit;
    public bool randomizeModel;
    public GameObject[] unitModels;

    public float minSpawnRate, maxSpawnRate;
    private float spawnTime = 0;

    public Action<GameObject> OnUnitSpawned;

    private void Start()
    {
        spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
    }

    private void Update()
    {
        if (spawnTime <= 0)
        {
            GameObject spawnedUnit = Instantiate(unit, transform.position, Quaternion.identity);
            if (randomizeModel)
            {
                GameObject model = Instantiate(unitModels[UnityEngine.Random.Range(0, unitModels.Length)], spawnedUnit.transform);
                model.transform.localScale *= 2;
            }

            OnUnitSpawned?.Invoke(spawnedUnit);

            spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
        }

        spawnTime -= Time.deltaTime;
    }
}
