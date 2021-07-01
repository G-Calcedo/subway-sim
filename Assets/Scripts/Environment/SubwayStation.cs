using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayStation : MonoBehaviour
{
    public static SubwayStation main;

    public Transform receptionist;
    public Transform exit;

    public GameObject platforms;
    private Platform[] _platforms;

    public GameObject ticketMachines;
    private TicketMachine[] _ticketMachines;

    public GameObject turnstiles;
    private Turnstile[] _turnstiles;

    private void Awake()
    {
        main = this;
        _platforms = platforms.GetComponentsInChildren<Platform>();
        _ticketMachines = ticketMachines.GetComponentsInChildren<TicketMachine>();
        _turnstiles = turnstiles.GetComponentsInChildren<Turnstile>();
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

    public Vector3 GetRandomTurnstilePosition()
    {
        Turnstile randomTurnstile = _turnstiles[Random.Range(0, _turnstiles.Length)];

        return randomTurnstile.entryPoint.transform.position;
    }
}
