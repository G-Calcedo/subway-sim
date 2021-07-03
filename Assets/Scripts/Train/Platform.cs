using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public TrainBehaviour train;
    public Vector3 alingment;

    private Collider platformCollider;
    private List<TrainUserBehaviour> passengers;

    private void Awake()
    {
        platformCollider = GetComponent<BoxCollider>();
        passengers = new List<TrainUserBehaviour>();

        train.platform = this;

        train.OnArrival += () =>
        {
            train.passengersLeft = passengers.Count;
            NotifyPassengers();
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Passenger") || other.CompareTag("Musician"))
        {
            TrainUserBehaviour passenger = other.gameObject.GetComponent<TrainUserBehaviour>();

            if (ReferenceEquals(this, passenger.spawnPlatform)) return;

            if (passenger.CurrentPlatform is null)
            {
                passenger.CurrentPlatform = this;
                passengers.Add(passenger);
            }
        }
    }

    public Vector3 GetRandomPosition()
    {
        Bounds bounds = platformCollider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.min.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public void NotifyPassengers()
    {
        foreach(TrainUserBehaviour passenger in passengers)
        {
            if (!ReferenceEquals(this, passenger.spawnPlatform))
            {
                passenger.readyToBoard = true;
            }
        }

        passengers.Clear();
    }
}
