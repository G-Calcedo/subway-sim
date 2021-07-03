using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraffitiSpawner : MonoBehaviour
{
    public GameObject unit;

    public float minSpawnRate, maxSpawnRate;
    private float spawnTime = 0;

    private void Start()
    {
        spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
    }

    private void Update()
    {
        if(spawnTime < 0 && SubwayStation.main.GetDayHour() >= 2f && SubwayStation.main.GetDayHour() <= 5f)
        {
            if (Random.Range(0, 100) < 20 && SubwayStation.main.IsGraffitiSpotAvailable())
            {
                GraffitiBehaviour graffitter = Instantiate(unit, transform.position, Quaternion.identity).GetComponent<GraffitiBehaviour>();
                graffitter.assignedSpot = SubwayStation.main.AssignRandomGraffitiSpot();
                graffitter.assignedSpot.inUse = true;
            }
            spawnTime = Random.Range(minSpawnRate, maxSpawnRate);
        }

        spawnTime -= Time.deltaTime;
    }
}
