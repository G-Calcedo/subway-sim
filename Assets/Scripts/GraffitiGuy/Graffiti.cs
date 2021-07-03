using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graffiti : MonoBehaviour
{
    public GameObject[] graffities;

    private void Start()
    {
        GameObject graffiti = Instantiate(graffities[Random.Range(0, graffities.Length)], transform);
        graffiti.transform.localScale *= 2.5f;
    }
}
