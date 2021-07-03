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

    public GameObject trash;

    private bool hasThanked;

    private void Start()
    {
        passengerBT = new BehaviourTreeEngine();
        passengerSM = new StateMachineEngine(true);
        movement = GetComponent<BasicMovement>();

        //transform.DOScaleY(1.2f, 0.25f).SetLoops(-1, LoopType.Yoyo);

        SequenceNode mainSequence = passengerBT.CreateSequenceNode("MainSequence", false);

        assignedPlatformPosition = SubwayStation.main.GetRandomPlatformPosition(spawnPlatform);

        if (fromOutside)
        {

            if (Random.Range(0, 100) < 0)
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
                    //assignedPlatformPosition = SubwayStation.main.GetRandomPlatformPosition();
                    movement.SetDestination(transform.position + new Vector3(4, 0, 0));
                },
                () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));
        }

        State goToDestination = passengerSM.CreateEntryState("GoToDestination",
            () =>
            {
                movement.SetDestination(assignedPlatformPosition);
                movement.ResumeMovement();

                if (hasThanked)
                {
                    assignedMusician.musicianSM.Fire("Thank");
                    hasThanked = false;
                }
                assignedMusician = null;
            });

        State listenToMusician = passengerSM.CreateState("ListenToMusic",
            () =>
            {
                movement.PauseMovement();
                transform.DOLookAt(assignedMusician.transform.position, 0.5f, AxisConstraint.None, Vector3.up);
            });

        State giveMoney = passengerSM.CreateState("GiveMoney",
            () =>
            {
                if (Random.Range(0, 100) < 30 && assignedMusician.isPlaying)
                {
                    Vector3 a = assignedMusician.hat.transform.position;
                    Vector3 b = transform.position;
                    Vector3 ba = (b - a).normalized;
                    movement.SetDestination(assignedMusician.hat.transform.position + ba * 1.5f);
                    movement.ResumeMovement();
                    hasThanked = true;
                }
                else
                {
                    passengerSM.Fire("KeepMoving");
                }
            });

        State throwTrash = passengerSM.CreateState("ThrowTrash",
            () =>
            {
                if(Random.Range(0, 100) < 10 && !IsValidZone() && movement.IsMoving())
                {
                    GameObject spawnedTrash = Instantiate(trash, transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                    spawnedTrash.transform.localScale *= Random.Range(1f, 1.5f);
                } 
            });

        Perception platformReached = passengerSM.CreatePerception<ValuePerception>(() => readyToBoard);
        Perception atractedByMusician = passengerSM.CreatePerception<ValuePerception>(() => !(assignedMusician is null));
        Perception listenTimer = passengerSM.CreatePerception<TimerPerception>(5);
        Perception moneyNotGiven = passengerSM.CreatePerception<PushPerception>();
        Perception moneyGiven = passengerSM.CreatePerception<ValuePerception>(() => !movement.IsMoving());
        Perception moneyPerc = passengerSM.CreateOrPerception<OrPerception>(moneyNotGiven, moneyGiven);
        Perception trashTimer = passengerSM.CreatePerception<TimerPerception>(Random.Range(2f, 10f));
        Perception trashThrowed = passengerSM.CreatePerception<IsInStatePerception>(passengerSM, "ThrowTrash");

        passengerSM.CreateTransition("Listen", goToDestination, atractedByMusician, listenToMusician);
        passengerSM.CreateTransition("StopListen", listenToMusician, listenTimer, giveMoney);
        passengerSM.CreateTransition("KeepMoving", giveMoney, moneyPerc, goToDestination);
        passengerSM.CreateTransition("StartThrowing", goToDestination, trashTimer, throwTrash);
        passengerSM.CreateTransition("StopThrowing", throwTrash, trashThrowed, goToDestination);

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

    private bool IsValidZone()
    {
        foreach (Collider c in Physics.OverlapSphere(transform.position, 2))
        {
            if (c.CompareTag("Trash") || c.CompareTag("Musician")) return true;
        }

        return false;
    }
}
