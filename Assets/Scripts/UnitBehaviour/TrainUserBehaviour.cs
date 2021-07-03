using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainUserBehaviour : MonoBehaviour
{
    protected BasicMovement movement;

    public TicketMachine assignedTicketMachine;
    public Turnstile assignedTurnstile;
    public Platform CurrentPlatform;
    public bool readyToBoard;
    public bool fromOutside;

    public Platform spawnPlatform;

    private void Awake()
    {
        movement = GetComponent<BasicMovement>();
    }
}
