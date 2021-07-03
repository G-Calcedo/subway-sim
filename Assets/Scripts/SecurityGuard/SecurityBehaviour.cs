using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityBehaviour : MonoBehaviour
{

    private StateMachineEngine securitySM;
    private RandomMovement randomMovement;

    //public Vector3 fightPos;
    //public Vector3 graffitiGuyPos;

    // Start is called before the first frame update
    void Awake()
    {
        securitySM = new StateMachineEngine();
        randomMovement = GetComponent<RandomMovement>();

        Perception workingShift = securitySM.CreatePerception<ValuePerception>(() => SubwayStation.main.GetDayHour() >= 8);
        Perception startResting = securitySM.CreatePerception<ValuePerception>(() => randomMovement.NearTarget(1));

        State patrol = securitySM.CreateEntryState("Patrol",
            () =>
            {
                randomMovement.active = true;
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

        securitySM.CreateTransition("EndWorkingShift", patrol, workingShift, goBackHome);
        securitySM.CreateTransition("Rest", goBackHome, startResting, rest);
    }

    // Update is called once per frame
    void Update()
    {
        securitySM.Update();
    }
}
