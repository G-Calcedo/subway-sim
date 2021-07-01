using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerBehaviour : MonoBehaviour
{
    private BehaviourTreeEngine passengerBT;
    private BasicMovement movement;
    private float valueAction;

    public Platform CurrentPlatform;
    public bool readyToBoard;

    public TicketMachine assignedTicketMachine;
    public Turnstile assignedTurnstile;

    public Musician assignedMusician;
    public bool asd;

    private void Awake()
    {
        passengerBT = new BehaviourTreeEngine();
        movement = GetComponent<BasicMovement>();
        valueAction = Random.Range(0, 100);

        SequenceNode mainSequence = passengerBT.CreateSequenceNode("MainSequence", false);

        if (Random.Range(0, 100) < 10)
        {
            mainSequence.AddChild(passengerBT.CreateLeafNode("AskingReceptionist",
            () => movement.SetDestination(SubwayStation.main.receptionist.position + new Vector3(0, 0, 2f)),
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));

            //Timer recepcion
            mainSequence.AddChild(passengerBT.CreateTimerNode("VamosJose", passengerBT.CreateLeafNode("BuyingTicket",
            () => movement.SetDestination(assignedTicketMachine.ticketPoint.transform.position),
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed), 1f));
        }
        else
        {
            mainSequence.AddChild(passengerBT.CreateLeafNode("BuyingTicket",
                () => movement.SetDestination(assignedTicketMachine.ticketPoint.transform.position),
                () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));
        }
        mainSequence.AddChild(passengerBT.CreateTimerNode("TicketDelay", passengerBT.CreateLeafNode("MoveToTurnstile",
            () =>
            {
                assignedTicketMachine.InUse = false;
                movement.SetDestination(assignedTurnstile.entryPoint.transform.position);
            },
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed), 0.01f));

        mainSequence.AddChild(passengerBT.CreateLeafNode("PassTurnstile",
            () =>
            {
                assignedTurnstile.InUse = false;
                movement.SetDestination(transform.position + new Vector3(4, 0, 0));
            },
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));


        /*
        if (valueAction < 20)
        {
            Debug.Log("Escucha musiquilla");
            if (Random.Range(0, 100) < 30)
            {
                Debug.Log("Deja propina");
            }
            else
            {
                Debug.Log("Cómeme la polla");
            }

        }
        else if (20 <= valueAction && valueAction < 30)
        {
            Debug.Log("Tirar basura");
        }
        else if (30 <= valueAction && valueAction < 50) { Debug.Log("Pegarse"); }
        else
        {
            mainSequence.AddChild(passengerBT.CreateLeafNode("DoNothing",
            () => { },
            () => ReturnValues.Succeed));
            Debug.Log("No hace nada");
        }
        */

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
        if (asd)
            Debug.Log(passengerBT.GetCurrentState().Name);
    }
}
