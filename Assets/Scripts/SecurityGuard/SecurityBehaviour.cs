using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecurityBehaviour : MonoBehaviour
{
    private StateMachineEngine securitySM;
    private RandomMovement randomMovement;

    public Transform scanCenter;
    //public Vector3 fightPos;
    //public Vector3 graffitiGuyPos;

    private GraffitiBehaviour target = null;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Awake()
    {
        securitySM = new StateMachineEngine();
        randomMovement = GetComponent<RandomMovement>();
        agent = GetComponent<NavMeshAgent>();

        Perception workingShift = securitySM.CreatePerception<ValuePerception>(() => SubwayStation.main.GetDayHour() >= 8);
        Perception startResting = securitySM.CreatePerception<ValuePerception>(() => randomMovement.NearTarget(1));
        Perception hasTarget = securitySM.CreatePerception<ValuePerception>(() => !(target is null));
        Perception hasNoTarget = securitySM.CreatePerception<ValuePerception>(() => target is null);

        State patrol = securitySM.CreateEntryState("Patrol",
            () =>
            {
                randomMovement.active = true;
                agent.speed = 10;
                CancelInvoke(nameof(ScanGraffiters));
                InvokeRepeating(nameof(ScanGraffiters), 0, 0.1f);
            });

        State chase = securitySM.CreateState("Chase",
            () =>
            {
                //Debug.Log("Te pillo el guarda");
                agent.speed = 12;
                //CancelInvoke();
                randomMovement.active = false;
                CancelInvoke(nameof(Chase));
                InvokeRepeating(nameof(Chase), 0, 0.25f);
                //randomMovement.SetDestination(target.transform.position);
            });

        State goBackHome = securitySM.CreateState("GoBackHome",
            () =>
            {
                CancelInvoke();
                randomMovement.active = false;
                randomMovement.SetDestination(SubwayStation.main.NearestCleanerSpot(transform.position));
            });

        State rest = securitySM.CreateState("Resting",
            () =>
            {
                SubwayStation.main.guardCount--;
                Destroy(gameObject);
            });

        securitySM.CreateTransition("ChaseGraffiter", patrol, hasTarget, chase);
        securitySM.CreateTransition("StopChase", chase, hasNoTarget, patrol);
        securitySM.CreateTransition("EndWorkingShift", patrol, workingShift, goBackHome);
        securitySM.CreateTransition("Rest", goBackHome, startResting, rest);
    }

    // Update is called once per frame
    void Update()
    {
        securitySM.Update();
    }

    private void ScanGraffiters()
    {
        foreach (Collider c in Physics.OverlapBox(scanCenter.position, new Vector3(5, 2, 5)))
        {
            if (c.CompareTag("Graffiter"))
            {
                GraffitiBehaviour graffiter = c.GetComponent<GraffitiBehaviour>();
                graffiter.scapeFrom = this;
                graffiter.caught = true;

                if (!(target is null)) target.OnDisappear -= StopChase;

                target = graffiter;
                target.OnDisappear += StopChase;

                break;
            }
        }
    }

    private void Chase()
    {
        randomMovement.SetDestination(target.transform.position);
    }

    private void StopChase()
    {
        CancelInvoke(nameof(Chase));
        target.OnDisappear -= StopChase;
        target = null;
    }
}
