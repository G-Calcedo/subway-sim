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
        if (target == this.target) return;

        this.target = target;
        navegation.SetDestination(target);
    }

    public void PauseMovement()
    {
        navegation.isStopped = true;
    }

    public void ResumeMovement()
    {
        navegation.isStopped = false;
    }

    /*
    public bool IsMoving()
    {
        Vector3 a = new Vector3(target.x, 0, target.z);
        Vector3 b = new Vector3(transform.position.x, 0, transform.position.z);
        return a != b;
    }*/

    public bool IsMoving()
    {
        if (!navegation.pathPending)
        {
            if (navegation.remainingDistance <= navegation.stoppingDistance)
            {
                if (!navegation.hasPath || navegation.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool NearTarget(float threshold)
    {
        Vector3 a = new Vector3(target.x, 0, target.z);
        Vector3 b = new Vector3(transform.position.x, 0, transform.position.z);

        return Vector3.Distance(a, b) <= threshold;
    }
}
