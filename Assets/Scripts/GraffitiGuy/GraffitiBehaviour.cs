using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using System;

public class GraffitiBehaviour : MonoBehaviour
{
    private StateMachineEngine graffitiSM;
    private BasicMovement movement;

    public GraffitiSpot assignedSpot;
    public Graffiti graffiti;

    public bool caught;

    private NavMeshAgent agent;
    public Action OnDisappear;
    public SecurityBehaviour scapeFrom;

    void Start()
    {
        graffitiSM = new StateMachineEngine();
        movement = GetComponent<BasicMovement>();
        agent = GetComponent<NavMeshAgent>();

        State goToDestination = graffitiSM.CreateEntryState("GoToDestination",
            () => movement.SetDestination(assignedSpot.transform.position));

        State drawGraffiti = graffitiSM.CreateState("DrawGraffiti",
            () =>
            {
                transform.DOLookAt(assignedSpot.spot.transform.position, 0.5f, AxisConstraint.Y);
            });

        State goToTunnel = graffitiSM.CreateState("GoToTunnel",
            () =>
            {
                Graffiti grf = Instantiate(graffiti,
                        assignedSpot.spot.transform.position,
                        assignedSpot.spot.transform.rotation);
                grf.GetComponent<Trash>().spot = assignedSpot;

                movement.SetDestination(SubwayStation.main.AssignRandomGraffitiTarget().transform.position);
            });

        State scape = graffitiSM.CreateState("Scape",
            () =>
            {
                agent.speed = 15;
                movement.SetDestination(SubwayStation.main.FurthestGraffiterScape(scapeFrom.transform.position));
            });

        State disappear = graffitiSM.CreateState("Disappear",
            () =>
            {
                OnDisappear?.Invoke();
                Destroy(gameObject);
            });

        Perception targetReached = graffitiSM.CreatePerception<ValuePerception>(() => !movement.IsMoving());
        Perception graffitiTimer = graffitiSM.CreatePerception<TimerPerception>(2);
        Perception isCaught = graffitiSM.CreatePerception<ValuePerception>(() => caught);

        graffitiSM.CreateTransition("MoveToGraffiti", goToDestination, targetReached, drawGraffiti);
        graffitiSM.CreateTransition("TryToScapeMove", goToDestination, isCaught, scape);
        graffitiSM.CreateTransition("TryToScapeDraw", drawGraffiti, isCaught, scape);
        graffitiSM.CreateTransition("MoveToTunnel", drawGraffiti, graffitiTimer, goToTunnel);
        graffitiSM.CreateTransition("TryToScapeTunnerl", goToTunnel, isCaught, scape);
        graffitiSM.CreateTransition("DisappearInTunnel_1", goToTunnel, targetReached, disappear);
        graffitiSM.CreateTransition("DisappearInTunnel_2", scape, targetReached, disappear);
    }

    void Update()
    {
        graffitiSM.Update();
    }
}
