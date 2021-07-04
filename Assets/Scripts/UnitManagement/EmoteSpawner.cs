using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteSpawner : MonoBehaviour
{
    public static EmoteSpawner spawner;

    private void Awake()
    {
        spawner = this;
    }

    public void SpawnEmote(GameObject parent, EmoteType emoteType, float lifetime)
    {
        Emote emote = Instantiate(Resources.Load("Emote", typeof(Emote)), parent.transform) as Emote;
        emote.SetEmote(emoteType);
        emote.lifetime = lifetime;
    }
}
