using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerBehaviour : MonoBehaviour
{
    private BehaviourTreeEngine passengerBT;

    private BasicMovement movement;

    private void Awake()
    {
        passengerBT = new BehaviourTreeEngine();
        movement = GetComponent<BasicMovement>();
    }

    private void Update()
    {
        passengerBT.Update();

        //passengerBT.CreateLeafNode("SetDestination", () => movement.SetDestination())
    }
}
