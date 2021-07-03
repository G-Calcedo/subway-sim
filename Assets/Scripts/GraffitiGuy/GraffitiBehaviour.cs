using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraffitiBehaviour : MonoBehaviour
{

    private BehaviourTreeEngine graffitiBT;
    private BasicMovement movement;

    public Platform CurrentPlatform;
    public bool readyToBoard;

    void Awake()
    {
        graffitiBT = new BehaviourTreeEngine();
        movement = GetComponent<BasicMovement>();
        /*
        SequenceNode graffitiSequence = graffitiBT.CreateSequenceNode("GraffitiSequence", false);

        graffitiSequence.AddChild(graffitiBT.CreateLeafNode("MoveToDestination",
           () => movement.SetDestination(SubwayStation.main.GetRandomPlatformPosition()),
           () => CurrentPlatform is null ? ReturnValues.Running : ReturnValues.Succeed));

        graffitiSequence.AddChild(graffitiBT.CreateLeafNode("WaitingForTrain",
            () => { },
            () => readyToBoard ? ReturnValues.Succeed : ReturnValues.Running));

        graffitiSequence.AddChild(graffitiBT.CreateLeafNode("Scape", () => movement.SetDestination(SubwayStation.main.exit.position),
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
