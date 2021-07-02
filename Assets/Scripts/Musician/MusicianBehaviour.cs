using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicianBehaviour : TrainUserBehaviour
{
    //public TicketMachine assignedTicketMachine;
    //public Turnstile assignedTurnstile;
    public MusicianSpot assignedMusicianSpot;

    public GameObject walk, playing, hat;

    private BehaviourTreeEngine musicianBT;
    private StateMachineEngine musicianSM;
    //private BasicMovement movement;

    //public Platform CurrentPlatform;
    //public bool readyToBoard;

    private void Start()
    {
        musicianBT = new BehaviourTreeEngine();
        musicianSM = new StateMachineEngine(true);
        movement = GetComponent<BasicMovement>();

        SequenceNode mainSequence = musicianBT.CreateSequenceNode("MainSequence", false);

        mainSequence.AddChild(musicianBT.CreateLeafNode("BuyingTicket",
               () => movement.SetDestination(assignedTicketMachine.ticketPoint.transform.position),
               () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));

        mainSequence.AddChild(musicianBT.CreateTimerNode("TicketDelay", musicianBT.CreateLeafNode("MoveToTurnstile",
                () =>
                {
                    assignedTicketMachine.InUse = false;
                    movement.SetDestination(assignedTurnstile.entryPoint.transform.position);
                },
                () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed), 0.01f));

        mainSequence.AddChild(musicianBT.CreateLeafNode("PassTurnstile",
            () =>
            {
                assignedTurnstile.InUse = false;
                movement.SetDestination(transform.position + new Vector3(4, 0, 0));
            },
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));

        mainSequence.AddChild(musicianBT.CreateLeafNode("MoveToSpot",
            () => movement.SetDestination(assignedMusicianSpot.transform.position),
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));

        mainSequence.AddChild(musicianBT.CreateLeafNode("PrepareInstrument",
            () =>
            {
                transform.DORotate(assignedMusicianSpot.alignment, 0.1f);
                InvokeRepeating(nameof(AtractPassengers), 0, 0.5f);
            },
            () => ReturnValues.Succeed));

        Perception moneyReceived = musicianSM.CreatePerception<PushPerception>();
        Perception keepPlaying = musicianSM.CreatePerception<TimerPerception>(0.5f);
        Perception stopPlaying = musicianSM.CreatePerception<TimerPerception>(15);

        Tween musicAnim = null;

        State playingMusic = musicianSM.CreateEntryState("Playing", () =>
        {
            keepPlaying.Reset();
            walk.SetActive(false);
            playing.SetActive(true);
            hat.SetActive(true);
            playing.transform.DOScale(200, 0.15f).OnComplete(() =>
            {
                musicAnim = playing.transform.DOScale(250, 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
            });
        });

        State thankingMoney = musicianSM.CreateState("Thanking", () =>
        {
            musicAnim.Kill();
            playing.transform.DOLocalJump(playing.transform.localPosition, 2, 1, 0.5f);
        });

        musicianSM.CreateTransition("Thank", playingMusic, moneyReceived, thankingMoney);
        musicianSM.CreateTransition("Play", thankingMoney, keepPlaying, playingMusic);

        mainSequence.AddChild(musicianBT.CreateSubBehaviour("PlayMusic", musicianSM));

        musicianSM.CreateExitTransition("StopPlaying", playingMusic, stopPlaying, ReturnValues.Succeed);

        mainSequence.AddChild(musicianBT.CreateLeafNode("StopPlaying",
            () =>
            {
                CancelInvoke(nameof(AtractPassengers));
                musicAnim.Kill();
                walk.SetActive(true);
                playing.SetActive(false);
                hat.SetActive(false);
                assignedMusicianSpot.InUse = false;
            },
            () => ReturnValues.Succeed));

        mainSequence.AddChild(musicianBT.CreateLeafNode("MoveToDestination",
           () => movement.SetDestination(SubwayStation.main.GetRandomPlatformPosition()),
           () => CurrentPlatform is null ? ReturnValues.Running : ReturnValues.Succeed));

        mainSequence.AddChild(musicianBT.CreateLeafNode("WaitingForTrain",
            () => { },
            () => readyToBoard ? ReturnValues.Succeed : ReturnValues.Running));

        mainSequence.AddChild(musicianBT.CreateLeafNode("EnterTrain",
            () => movement.SetDestination(CurrentPlatform.train.GetClosestEntrance(transform.position)),
            () => ReturnValues.Succeed));

        musicianBT.SetRootNode(mainSequence);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            musicianSM.Fire("Thank");
        }

        musicianBT.Update();
        musicianSM.Update();
    }

    private void AtractPassengers()
    {
        foreach (Collider passenger in Physics.OverlapSphere(transform.position, 15))
        {
            if (passenger.CompareTag("Passenger") && Random.Range(0, 100) < 10)
            {
                passenger.GetComponent<PassengerBehaviour>().assignedMusician = this;
                Debug.Log("Cazado");
            }
        }
    }
}
