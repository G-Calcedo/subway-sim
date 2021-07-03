using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public GameObject musicianSpots;
    private MusicianSpot[] _musicianSpots;

    public int cleanerCount, guardCount;
    //public GameObject cleanerSpots;
    public Transform[] _cleanerSpots;

    public ClockTimer timeControl;

    public Reception reception;

    private void Awake()
    {
        main = this;
        _platforms = platforms.GetComponentsInChildren<Platform>();
        _ticketMachines = ticketMachines.GetComponentsInChildren<TicketMachine>();
        _turnstiles = turnstiles.GetComponentsInChildren<Turnstile>();
        _musicianSpots = musicianSpots.GetComponentsInChildren<MusicianSpot>();
       // _cleanerSpots = cleanerSpots.GetComponentsInChildren<Transform>();
    }

    public Vector3 GetRandomPlatformPosition(Platform spawnPlatform)
    {
        if(spawnPlatform is null)
        {
            Platform randomPlatform = _platforms[UnityEngine.Random.Range(0, _platforms.Length)];

            return randomPlatform.GetRandomPosition();
        }

        foreach (Platform p in Shuffle(_platforms))
        {
            if(!ReferenceEquals(p, spawnPlatform))
                return p.GetRandomPosition();
        }

        return Vector3.zero;
    }

    public bool IsTicketMachineAvailable()
    {
        foreach(TicketMachine tm in _ticketMachines)
        {
            if (!tm.InUse)
            {
                return true;
            }
        }

        return false;
    }

    public TicketMachine AssignRandomTicketMachine()
    {
        foreach(TicketMachine tm in Shuffle(_ticketMachines))
        {
            if (!tm.InUse)
            {
                return tm;
            }
        }

        return null;
    }

    public bool IsTurnstileAvailable()
    {
        foreach(Turnstile ts in _turnstiles)
        {
            if (!ts.InUse)
            {
                return true;
            }
        }

        return false;
    }

    public Turnstile AssignRandomTurnstile()
    {
        foreach (Turnstile ts in Shuffle(_turnstiles))
        {
            if (!ts.InUse)
            {
                return ts;
            }
        }

        return null;
    }

    public bool IsMusicianSpotAvailable()
    {
        foreach(MusicianSpot ms in _musicianSpots)
        {
            if (!ms.InUse)
            {
                return true;
            }
        }

        return false;
    }

    public MusicianSpot AssignRandomMusicianSpot()
    {
        foreach (MusicianSpot ms in Shuffle(_musicianSpots))
        {
            if (!ms.InUse)
            {
                return ms;
            }
        }

        return null;
    }

    public Vector3 NearestCleanerSpot(Vector3 origin)
    {
        Vector3 spot = Vector3.zero;
        float distance = Mathf.Infinity;

        foreach(Transform sp in _cleanerSpots)
        {
            float currentDistance = Vector3.Distance(origin, sp.position);
            if(currentDistance < distance)
            {
                spot = sp.position;
                distance = currentDistance;
            }
        }

        return spot;
    }

    private T[] Shuffle<T>(T[] texts)
    {
        for (int t = 0; t < texts.Length; t++)
        {
            T tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
        return texts;
    }
}
