using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainUnitSpawner : MonoBehaviour
{
    public GameObject passengerUnit;
    public GameObject musicianUnit;
    public GameObject[] unitModels;

    public int minBatch, maxBatch;

    private TrainBehaviour train;

    private void Awake()
    {
        train = GetComponentInParent<TrainBehaviour>();
        train.OnPassengersLeave += Spawn;
    }

    private void Spawn()
    {
        int limit = Random.Range(minBatch, maxBatch);

        for(int i = 0; i < limit; i++)
        {
            if (Random.Range(0, 100) < 5 && SubwayStation.main.IsMusicianSpotAvailable())
            {
                SpawnMusician();
            }
            else
            {
                SpawnPassenger();
            }
        }
    }

    private void SpawnPassenger()
    {
        GameObject spawnedUnit = Instantiate(passengerUnit, transform.position, Quaternion.identity);
        GameObject model = Instantiate(unitModels[UnityEngine.Random.Range(0, unitModels.Length)], spawnedUnit.transform);
        model.transform.localScale *= 2;

        PassengerBehaviour passenger = spawnedUnit.GetComponent<PassengerBehaviour>();
        passenger.spawnPlatform = train.platform;
        passenger.fromOutside = false;
    }

    private void SpawnMusician()
    {
        GameObject spawnedUnit = Instantiate(musicianUnit, transform.position, Quaternion.identity);

        MusicianBehaviour musician = spawnedUnit.GetComponent<MusicianBehaviour>();
        musician.assignedMusicianSpot = SubwayStation.main.AssignRandomMusicianSpot();
        musician.assignedMusicianSpot.InUse = true;
        musician.spawnPlatform = train.platform;
        musician.fromOutside = false;
    }
}
