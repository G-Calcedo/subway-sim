using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayStation : MonoBehaviour
{
    public static SubwayStation main;

    public GameObject platforms;
    private Platform[] _platforms;

    private void Awake()
    {
        main = this;
        _platforms = GetComponentsInChildren<Platform>();
    }

    public Vector3 GetRandomPlatformPosition()
    {
        Platform randomPlatform = _platforms[Random.Range(0, _platforms.Length)];

        return randomPlatform.GetRandomPosition();
    }
}
