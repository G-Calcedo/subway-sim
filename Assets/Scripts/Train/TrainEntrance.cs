using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainEntrance : MonoBehaviour
{
    private TrainBehaviour train;

    private void Awake()
    {
        train = GetComponentInParent<TrainBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Passenger"))
        {
            train.passengerCount--;
            Destroy(other.gameObject);
        }
    }
}
