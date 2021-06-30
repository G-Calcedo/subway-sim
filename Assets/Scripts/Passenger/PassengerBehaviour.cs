using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerBehaviour : MonoBehaviour
{
    private BehaviourTreeEngine passengerBT;

    private BasicMovement movement;
    public Platform CurrentPlatform;
    public bool isReady;

    private void Awake()
    {
        passengerBT = new BehaviourTreeEngine();
        movement = GetComponent<BasicMovement>();

        SequenceNode mainSequence = passengerBT.CreateSequenceNode("MainSequence", false);

        mainSequence.AddChild(passengerBT.CreateLeafNode("MoveToDestination",
            () => movement.SetDestination(SubwayStation.main.GetRandomPlatformPosition()),
            () => CurrentPlatform == null ? ReturnValues.Running : ReturnValues.Succeed));

        mainSequence.AddChild(passengerBT.CreateLeafNode("WaitingForTrain",
            () => { },
            () => isReady ? ReturnValues.Succeed : ReturnValues.Running));

        mainSequence.AddChild(passengerBT.CreateLeafNode("EnterTrain",
            () => movement.SetDestination(CurrentPlatform.train.GetClosestEntrance(transform.position)),
            () => ReturnValues.Succeed));

        passengerBT.SetRootNode(mainSequence);
    }

    private void Update()
    {
        passengerBT.Update();
    }
}
