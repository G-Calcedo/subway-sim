using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicMovement : MonoBehaviour
{
    private NavMeshAgent navegation;

    private void Awake()
    {
        navegation = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 target)
    {
        navegation.SetDestination(target);
    }
}
