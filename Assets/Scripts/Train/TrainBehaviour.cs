using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBehaviour : MonoBehaviour
{
    public Transform inLocation, stationLocation, outLocation;
    public GameObject entryPoints;

    public Action OnPassengersLeave;
    public Action OnArrival;

    private StateMachineEngine trainSM;
    private MeshRenderer rend;
    public int passengersLeft;

    public Platform platform;

    private void Awake()
    {
        rend = GetComponentInChildren<MeshRenderer>();
        entryPoints.SetActive(false);
    }

    private void Start()
    {
        trainSM = new StateMachineEngine();

        Perception timer = trainSM.CreatePerception<TimerPerception>(UnityEngine.Random.Range(1f, 10f));
        Perception passengersLeftTimer = trainSM.CreatePerception<TimerPerception>(1);
        Perception stationEntered = trainSM.CreatePerception<PushPerception>();
        Perception stationExited = trainSM.CreatePerception<PushPerception>();
        Perception departureReady = trainSM.CreatePerception<ValuePerception>(() => passengersLeft == 0);
        Perception dayTime = trainSM.CreatePerception<ValuePerception>(() =>
         (SubwayStation.main.timeControl.dayTimerNormalized * SubwayStation.main.timeControl.hoursDay > 8f) ||
         (SubwayStation.main.timeControl.dayTimerNormalized * SubwayStation.main.timeControl.hoursDay < 1f));
        //Perception trainOnService = trainSM.CreateAndPerception<AndPerception>(timer, dayTime);

        State onTravel = trainSM.CreateEntryState("OnTravel", () =>
        {
            timer.Reset();
            transform.position = inLocation.position;
            rend.enabled = false;
        });
        State enterStation = trainSM.CreateState("EnterStation", () =>
        {
            rend.enabled = true;
            transform.DOMove(stationLocation.position, 3).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                trainSM.Fire(stationEntered);
            });
        });
        State passengerLeave = trainSM.CreateState("PassengersLeave", () =>
        {
            passengersLeftTimer.Reset();
            OnPassengersLeave?.Invoke();
        });
        State onStation = trainSM.CreateState("OnStation", () =>
        {
            departureReady.Reset();
            OnArrival?.Invoke();
            entryPoints.SetActive(true);
        });
        State exitStation = trainSM.CreateState("ExitStation", () =>
        {
            entryPoints.SetActive(false);
            transform.DOMove(outLocation.position, 3).SetEase(Ease.InCubic).OnComplete(() =>
            {
                trainSM.Fire(stationExited);
            });
        });
        State serviceCheck = trainSM.CreateState("ServiceCheck", () =>
        {
            rend.enabled = false;
        });

        trainSM.CreateTransition("Enter", onTravel, timer, enterStation);
        trainSM.CreateTransition("EnterFinished", enterStation, stationEntered, passengerLeave);
        trainSM.CreateTransition("ReadyToBoard", passengerLeave, passengersLeftTimer, onStation);
        trainSM.CreateTransition("Exit", onStation, departureReady, exitStation);
        trainSM.CreateTransition("ExitFinished", exitStation, stationExited, serviceCheck);
        trainSM.CreateTransition("Service", serviceCheck, dayTime, onTravel);
    }

    private void Update()
    {
        trainSM.Update();
    }

    public Vector3 GetClosestEntrance(Vector3 passengerPos)
    {
        Vector3 closestEntrance = passengerPos;
        float closestDistance = Mathf.Infinity;

        foreach (Transform entrance in entryPoints.transform)
        {
            float distance = Vector3.Distance(passengerPos, entrance.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEntrance = entrance.position;
            }
        }

        return closestEntrance;
    }
}
