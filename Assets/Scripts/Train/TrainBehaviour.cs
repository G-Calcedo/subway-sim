using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBehaviour : MonoBehaviour
{
    public Transform inLocation, stationLocation, outLocation;
    public GameObject entryPoints;

    public Action OnArrival, OnDeparture;

    private StateMachineEngine trainSM;
    private MeshRenderer rend;
    public int passengerCount;

    private void Awake()
    {
        rend = GetComponentInChildren<MeshRenderer>();
        entryPoints.SetActive(false);
    }

    private void Start()
    {
        trainSM = new StateMachineEngine();

        Perception timer = trainSM.CreatePerception<TimerPerception>(UnityEngine.Random.Range(1f, 10f));
        Perception stationEntered = trainSM.CreatePerception<PushPerception>();
        Perception stationExited = trainSM.CreatePerception<PushPerception>();
        Perception departureReady = trainSM.CreatePerception<ValuePerception>(() => passengerCount == 0);
        Perception departureTimer = trainSM.CreatePerception<TimerPerception>(15);
        Perception departurePerc = trainSM.CreateOrPerception<OrPerception>(departureReady, departureTimer);

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
        State onStation = trainSM.CreateState("OnStation", () =>
        {
            departureReady.Reset();
            departureTimer.Reset();
            OnArrival?.Invoke();
            entryPoints.SetActive(true);
        });
        State exitStation = trainSM.CreateState("ExitStation", () =>
        {
            entryPoints.SetActive(false);
            OnDeparture?.Invoke();

            transform.DOMove(outLocation.position, 3).SetEase(Ease.InCubic).OnComplete(() =>
            {
                trainSM.Fire(stationExited);
            });
        });

        trainSM.CreateTransition("Enter", onTravel, timer, enterStation);
        trainSM.CreateTransition("EnterFinished", enterStation, stationEntered, onStation);
        trainSM.CreateTransition("Exit", onStation, departureReady, exitStation);
        trainSM.CreateTransition("ExitFinished", exitStation, stationExited, onTravel);
    }

    private void Update()
    {
        trainSM.Update();
    }

    public Vector3 GetClosestEntrance(Vector3 passengerPos)
    {
        Vector3 closestEntrance = passengerPos;
        float closestDistance = Mathf.Infinity;

        foreach(Transform entrance in entryPoints.transform)
        {
            float distance = Vector3.Distance(passengerPos, entrance.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestEntrance = entrance.position;
            }
        }

        return closestEntrance;
    }
}
