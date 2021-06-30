using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicMovement : MonoBehaviour
{
    private NavMeshAgent navegation;
    private Vector3 target;

    private void Awake()
    {
        navegation = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 target)
    {
        this.target = target;
        navegation.SetDestination(target);
    }

    public bool IsMoving()
    {
        Vector3 a = new Vector3(target.x, 0, target.z);
        Vector3 b = new Vector3(transform.position.x, 0, transform.position.z);
        return a != b;
    }
}
