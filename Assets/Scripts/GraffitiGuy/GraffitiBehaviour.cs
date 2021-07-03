using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraffitiBehaviour : MonoBehaviour
{
    private StateMachineEngine graffitiSM;
    private BasicMovement movement;

    void Awake()
    {
        graffitiSM = new StateMachineEngine();
        movement = GetComponent<BasicMovement>();

        State goToDestination = graffitiSM.CreateEntryState("GoToDestination",
            () => { });
    }

    void Update()
    {
        
    }
}
