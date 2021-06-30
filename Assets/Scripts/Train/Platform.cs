using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public TrainBehaviour train;

    private Collider platformCollider;
    private List<PassengerBehaviour> passengers;

    private void Awake()
    {
        platformCollider = GetComponent<BoxCollider>();
        passengers = new List<PassengerBehaviour>();

        train.OnArrival += () =>
        {
            train.passengersLeft = passengers.Count;
            NotifyPassengers();
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Passenger"))
        {
            PassengerBehaviour passenger = other.gameObject.GetComponent<PassengerBehaviour>();
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
        foreach(PassengerBehaviour passenger in passengers)
        {
            passenger.readyToBoard = true;
        }

        passengers.Clear();
    }
}
