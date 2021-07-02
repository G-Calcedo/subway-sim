using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerBehaviour : TrainUserBehaviour
{
    private BehaviourTreeEngine passengerBT;
    private StateMachineEngine passengerSM;
    //private BasicMovement movement;

    //public Platform CurrentPlatform;
    //public bool readyToBoard;

    //public TicketMachine assignedTicketMachine;
    //public Turnstile assignedTurnstile;

    public MusicianBehaviour assignedMusician;
    public Vector3 assignedPlatformPosition;

    private void Awake()
    {
        passengerBT = new BehaviourTreeEngine();
        passengerSM = new StateMachineEngine(true);
        movement = GetComponent<BasicMovement>();

        //transform.DOScaleY(1.2f, 0.25f).SetLoops(-1, LoopType.Yoyo);

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
                assignedPlatformPosition = SubwayStation.main.GetRandomPlatformPosition();
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

        Perception platformReached = passengerSM.CreatePerception<ValuePerception>(() => readyToBoard);
        Perception atractedByMusicion = passengerSM.CreatePerception<ValuePerception>(() => !(assignedMusician is null));
        Perception listenTimer = passengerSM.CreatePerception<TimerPerception>(5f);

        State goToDestination = passengerSM.CreateEntryState("GoToDestination",
            () =>
            {
                movement.SetDestination(assignedPlatformPosition);
                movement.ResumeMovement();
                assignedMusician = null;
            });

        State listenToMusician = passengerSM.CreateState("ListenToMusic",
            () =>
            {
                Debug.Log("ME HAN CAZADO");
                movement.PauseMovement();
                transform.DOLookAt(assignedMusician.transform.position, 0.25f, AxisConstraint.None, Vector3.up);
            });

        passengerSM.CreateTransition("Listen", goToDestination, atractedByMusicion, listenToMusician);
        passengerSM.CreateTransition("StopListen", listenToMusician, listenTimer, goToDestination);

        mainSequence.AddChild(passengerBT.CreateSubBehaviour("Travelling", passengerSM));

        passengerSM.CreateExitTransition("PlatformReached", goToDestination, platformReached, ReturnValues.Succeed);

        mainSequence.AddChild(passengerBT.CreateLeafNode("EnterTrain",
            () => movement.SetDestination(CurrentPlatform.train.GetClosestEntrance(transform.position)),
            () => ReturnValues.Succeed));

        passengerBT.SetRootNode(mainSequence);
    }

    private void Update()
    {
        passengerBT.Update();
        passengerSM.Update();
    }
}
