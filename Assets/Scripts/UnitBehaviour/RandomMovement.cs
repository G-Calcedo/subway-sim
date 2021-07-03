using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public float wanderRadius;
    public float wanderTimer;

    public bool active;

    private Vector3 target;
    private NavMeshAgent agent;
    private float timer;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if ((timer >= wanderTimer || !IsMoving()) && active)
        {
            Vector3 newPos = RandomNavSphere(SubwayStation.main.transform.position, wanderRadius, -1);
            target = newPos;
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    public void SetDestination(Vector3 pos)
    {
        agent.SetDestination(pos);
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
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
