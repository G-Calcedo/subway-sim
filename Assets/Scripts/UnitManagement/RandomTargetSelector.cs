using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargetSelector : MonoBehaviour
{
    public List<Transform> targets;

    public UnitSpawner trigger;


    private void Start()
    {
        trigger.OnUnitSpawned += SetRandomTarget;
    }

    private void SetRandomTarget(GameObject unit)
    {
        unit.GetComponent<BasicMovement>().SetDestination(targets[Random.Range(0, targets.Count)].position);
    }
}
