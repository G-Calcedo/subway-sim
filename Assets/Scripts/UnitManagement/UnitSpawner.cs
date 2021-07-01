using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject passengerUnit;
    public GameObject musicianUnit;
    public GameObject[] unitModels;

    public float minSpawnRate, maxSpawnRate;
    private float spawnTime;

    public Action<GameObject> OnUnitSpawned;

    private void Start()
    {
        spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
    }

    private void Update()
    {
        if (spawnTime <= 0 && SubwayStation.main.IsTicketMachineAvailable() && SubwayStation.main.IsTurnstileAvailable())
        {
            if (UnityEngine.Random.Range(0, 100) < 10 && SubwayStation.main.IsMusicianSpotAvailable())
            {
                SpawnMusician();
            }
            else
            {
                SpawnPassenger();
            }

            spawnTime = UnityEngine.Random.Range(minSpawnRate, maxSpawnRate);
        }

        spawnTime -= Time.deltaTime;
    }

    private void SpawnPassenger()
    {
        GameObject spawnedUnit = Instantiate(passengerUnit, transform.position, Quaternion.identity);
        GameObject model = Instantiate(unitModels[UnityEngine.Random.Range(0, unitModels.Length)], spawnedUnit.transform);
        model.transform.localScale *= 2;

        OnUnitSpawned?.Invoke(spawnedUnit);

        PassengerBehaviour passenger = spawnedUnit.GetComponent<PassengerBehaviour>();
        passenger.assignedTicketMachine = SubwayStation.main.AssignRandomTicketMachine();
        passenger.assignedTicketMachine.InUse = true;
        passenger.assignedTurnstile = SubwayStation.main.AssignRandomTurnstile();
        passenger.assignedTurnstile.InUse = true;
    }

    private void SpawnMusician()
    {
        GameObject spawnedUnit = Instantiate(musicianUnit, transform.position, Quaternion.identity);

        Musician musician = spawnedUnit.GetComponent<Musician>();
        musician.assignedTicketMachine = SubwayStation.main.AssignRandomTicketMachine();
        musician.assignedTicketMachine.InUse = true;
        musician.assignedTurnstile = SubwayStation.main.AssignRandomTurnstile();
        musician.assignedTurnstile.InUse = true;
        musician.assignedMusicianSpot = SubwayStation.main.AssignRandomMusicianSpot();
        musician.assignedMusicianSpot.InUse = true;
    }
}
