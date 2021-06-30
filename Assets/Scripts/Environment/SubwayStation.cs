using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayStation : MonoBehaviour
{
    public static SubwayStation main;

    public GameObject platforms;
    public Platform[] _platforms;

    public GameObject ticketMachines;
    public TicketMachine[] _ticketMachines;

    private void Awake()
    {
        main = this;
        _platforms = platforms.GetComponentsInChildren<Platform>();
        _ticketMachines = ticketMachines.GetComponentsInChildren<TicketMachine>();
    }

    public Vector3 GetRandomPlatformPosition()
    {
        Platform randomPlatform = _platforms[Random.Range(0, _platforms.Length)];

        return randomPlatform.GetRandomPosition();
    }

    public Vector3 GetRandomTicketMachinePosition()
    {
        TicketMachine randomTicketMachine = _ticketMachines[Random.Range(0, _ticketMachines.Length)];

        return randomTicketMachine.ticketPoint.transform.position;
    }
}
