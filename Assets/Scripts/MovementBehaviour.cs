using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementBehaviour : MonoBehaviour
{
    public NavMeshAgent navegation;

    public List<Transform> targets;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            navegation.SetDestination(targets[0].position);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            navegation.SetDestination(targets[1].position);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            navegation.SetDestination(targets[2].position);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            navegation.SetDestination(targets[3].position);
    }
}
