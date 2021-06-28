using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public Transform inLocation, stationLocation, outLocation;

    private StateMachineEngine trainSM;
    private MeshRenderer rend;

    private void Awake()
    {
        rend = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        trainSM = new StateMachineEngine();

        Perception timer = trainSM.CreatePerception<TimerPerception>(Random.Range(1, 5));
        Perception stationEntered = trainSM.CreatePerception<PushPerception>();
        Perception stationExited = trainSM.CreatePerception<PushPerception>();

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
            timer.Reset();
            Debug.Log("SUBANSE PUTOS");
        });
        State exitStation = trainSM.CreateState("ExitStation", () =>
        {
            transform.DOMove(outLocation.position, 3).SetEase(Ease.InCubic).OnComplete(() =>
            {
                trainSM.Fire(stationExited);
            });
        });

        trainSM.CreateTransition("Enter", onTravel, timer, enterStation);
        trainSM.CreateTransition("EnterFinished", enterStation, stationEntered, onStation);
        trainSM.CreateTransition("Exit", onStation, timer, exitStation);
        trainSM.CreateTransition("ExitFinished", exitStation, stationExited, onTravel);
    }

    private void Update()
    {
        trainSM.Update();
    }
}
