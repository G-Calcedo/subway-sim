using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public Transform inLocation, stationLocation, outLocation;

    private StateMachineEngine trainSM;

    private void Start()
    {
        trainSM = new StateMachineEngine();

        Perception timer = trainSM.CreatePerception<TimerPerception>(3);
        Perception stationEntered = trainSM.CreatePerception<PushPerception>();
        Perception stationExited = trainSM.CreatePerception<PushPerception>();

        State onTravel = trainSM.CreateEntryState("OnTravel", () =>
        {
            timer.Reset();
            transform.position = inLocation.position;
        });
        State enterStation = trainSM.CreateState("EnterStation", () =>
        {
            transform.DOMove(stationLocation.position, 1).SetEase(Ease.OutExpo).OnComplete(() =>
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
            transform.DOMove(outLocation.position, 1).SetEase(Ease.InExpo).OnComplete(() =>
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
