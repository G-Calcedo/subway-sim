using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PremadeMovement : MonoBehaviour
{

    private Vector3[] pointsToGo = new Vector3[4];
    private bool[] visitedPoints = new bool[4];
    private Vector3 nearestTransform;
    private Vector3 spawnPos;
    private BasicMovement movement;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<BasicMovement>();
        spawnPos = transform.position;

        pointsToGo[0] = new Vector3(68.64f, -26.55f, -3.47f);
        pointsToGo[1] = new Vector3(-3.26f, -2.73f, -2.068f);
        pointsToGo[2] = new Vector3(102.67f, -42.6f, 26.82f);
        pointsToGo[3] = new Vector3(106.74f, -42.6f, -14.138f);
        nearestTransform = pointsToGo[0];
        LookForClosestPoint();
    }

    public void LookForClosestPoint()
    {
        float closestDistance = Vector3.Distance(transform.position, pointsToGo[0]);
        if (!IsAllMissionComplete())
        {
            for (int i = 0; i < pointsToGo.Length; i++)
            {
                if (!visitedPoints[i] && Vector3.Distance(transform.position, pointsToGo[i]) < closestDistance)
                {
                    nearestTransform = pointsToGo[i];
                    visitedPoints[i] = true;
                }

            }
            movement.SetDestination(nearestTransform);
        }
        else
        {
            movement.SetDestination(spawnPos);
        }
    }

    private bool IsAllMissionComplete()
    {
        for (int i = 0; i < visitedPoints.Length; ++i)
        {
            if (visitedPoints[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        if(transform.position == nearestTransform)
        {
            LookForClosestPoint();
        }
    }

}
