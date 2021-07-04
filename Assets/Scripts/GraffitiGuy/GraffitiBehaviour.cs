using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GraffitiBehaviour : MonoBehaviour
{
    private StateMachineEngine graffitiSM;
    private BasicMovement movement;

    public GraffitiSpot assignedSpot;
    public Graffiti graffiti;

    void Start()
    {
        graffitiSM = new StateMachineEngine();
        movement = GetComponent<BasicMovement>();

        State goToDestination = graffitiSM.CreateEntryState("GoToDestination",
            () => movement.SetDestination(assignedSpot.transform.position));

        State drawGraffiti = graffitiSM.CreateState("DrawGraffiti",
            () =>
            {
                transform.DOLookAt(assignedSpot.spot.transform.position, 0.5f, AxisConstraint.Y);
            });

        State scape = graffitiSM.CreateState("Scape",
            () =>
            {
                Graffiti grf = Instantiate(graffiti,
                    assignedSpot.spot.transform.position,
                    assignedSpot.spot.transform.rotation);
                grf.GetComponent<Trash>().spot = assignedSpot;
                movement.SetDestination(SubwayStation.main.AssignRandomGraffitiTarget().transform.position);
            });

        State disappear = graffitiSM.CreateState("Disappear",
            () => Destroy(gameObject));

        Perception targetReached = graffitiSM.CreatePerception<ValuePerception>(() => !movement.IsMoving());
        Perception graffitiTimer = graffitiSM.CreatePerception<TimerPerception>(2);

        graffitiSM.CreateTransition("MoveToGraffiti", goToDestination, targetReached, drawGraffiti);
        graffitiSM.CreateTransition("TryToScape", drawGraffiti, graffitiTimer, scape);
        graffitiSM.CreateTransition("DisapperInTunnel", scape, targetReached, disappear);
    }

    void Update()
    {
        graffitiSM.Update();
    }
}
