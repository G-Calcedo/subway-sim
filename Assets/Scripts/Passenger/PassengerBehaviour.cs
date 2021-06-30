using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerBehaviour : MonoBehaviour
{
    private BehaviourTreeEngine passengerBT;
    private BasicMovement movement;

    public Platform CurrentPlatform;
    public bool readyToBoard;

    private void Awake()
    {
        passengerBT = new BehaviourTreeEngine();
        movement = GetComponent<BasicMovement>();

        SequenceNode mainSequence = passengerBT.CreateSequenceNode("MainSequence", false);

        //mainSequence.AddChild(passengerBT.CreateLeafNode("BuyingTicket",
        //    () => movement.SetDestination(SubwayStation.main.GetRandomTicketMachinePosition()),
        //    () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));

        //mainSequence.AddChild(passengerBT.CreateTimerNode("TicketDelay",
        //    (passengerBT.CreateLeafNode("MoveToDestination",
        //    () => movement.SetDestination(SubwayStation.main.GetRandomPlatformPosition()),
        //    () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed)),
        //    0.01f));

        mainSequence.AddChild(passengerBT.CreateLeafNode("MoveToDestination",
            () => movement.SetDestination(SubwayStation.main.GetRandomPlatformPosition()),
            () => CurrentPlatform is null ? ReturnValues.Running : ReturnValues.Succeed));

        mainSequence.AddChild(passengerBT.CreateLeafNode("WaitingForTrain",
            () => { },
            () => readyToBoard ? ReturnValues.Succeed : ReturnValues.Running));

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
