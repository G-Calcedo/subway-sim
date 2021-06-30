using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargetSelector : MonoBehaviour
{
    public List<Transform> targets;

    public UnitSpawner trigger;

    public List<TransformList> targetsTEST;

    private void Start()
    {
        trigger.OnUnitSpawned += SetRandomTarget;
    }

    private void SetRandomTarget(GameObject unit)
    {
        //unit.GetComponent<BasicMovement>().SetDestination(targets[Random.Range(0, targets.Count)].position);

        int randomValue = Random.Range(0, targetsTEST.Count);
        //Debug.Log(randomValue + " "+ targetsTEST.Count);
        TransformList targetRandom = targetsTEST[randomValue];

        unit.GetComponent<BasicMovement>().SetDestination(new Vector3(Random.Range(targetRandom.list[1].position.x, targetRandom.list[0].position.x), targetRandom.list[0].position.y, 
            Random.Range(targetRandom.list[0].position.z, targetRandom.list[1].position.z)));

    }
}

[System.Serializable]
public class TransformList
{
    public List<Transform> list;
}
