using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargetZone : MonoBehaviour
{
    public UnitSpawner trigger;

    public List<TransformList> targetsTEST;

    private void Start()
    {
        trigger.OnUnitSpawned += SetRandomTarget;
    }

    private void SetRandomTarget(GameObject unit)
    {
        TransformList targetRandom = targetsTEST[Random.Range(0, targetsTEST.Count)];

        unit.GetComponent<BasicMovement>().SetDestination(new Vector3(Random.Range(targetRandom.list[0].position.x, targetRandom.list[1].position.x), targetRandom.list[0].position.y, 
            Random.Range(targetRandom.list[1].position.z, targetRandom.list[0].position.z)));

    }
}

[System.Serializable]
public class TransformList
{
    public List<Transform> list;
}
