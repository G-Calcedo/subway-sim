using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityBehaviour : MonoBehaviour
{

    private BehaviourTreeEngine securityBT;
    private BasicMovement movement;

    public Vector3 fightPos;
    public Vector3 graffitiGuyPos;

    // Start is called before the first frame update
    void Awake()
    {
        securityBT = new BehaviourTreeEngine();
        movement = GetComponent<BasicMovement>();
        SequenceNode securitySequence = securityBT.CreateSequenceNode("SecuritySequence", false);

        securitySequence.AddChild(securityBT.CreateLeafNode("GointToFight", () => movement.SetDestination(fightPos),
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));

        securitySequence.AddChild(securityBT.CreateLeafNode("ChaseGraffitiGuy", () => movement.SetDestination(graffitiGuyPos),
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));

        securitySequence.AddChild(securityBT.CreateLeafNode("TakeHimToExit", () => movement.SetDestination(SubwayStation.main.exit.position),
            () => movement.IsMoving() ? ReturnValues.Running : ReturnValues.Succeed));

    }

    // Update is called once per frame
    void Update()
    {

    }
}
