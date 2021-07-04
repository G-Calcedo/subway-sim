using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EmoteType
{
    Music,
    Money,
    Thank,
    Happy,
    Angry
}

public class Emote : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite music, money, thank, happy, angry;

    private Tween anim;
    public float lifetime;

    private Sprite GetEmote(EmoteType emote)
    {
        return emote switch
        {
            EmoteType.Music => music,
            EmoteType.Money => money,
            EmoteType.Thank => thank,
            EmoteType.Happy => happy,
            EmoteType.Angry => angry,
            _ => null
        };
    }

    public void SetEmote(EmoteType emote)
    {
        rend.sprite = GetEmote(emote);
    }

    private void Awake()
    {
        //transform.position += new Vector3(0, 5, 0);
        rend = GetComponent<SpriteRenderer>();      

        anim = transform.DOScale(20,  0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBounce);       
    }

    private void Update()
    {
        //transform.LookAt(Camera.main.transform.position);
        transform.position = transform.parent.position + new Vector3(0, 5, 0);
        transform.rotation = Quaternion.Euler(30, -45, 0);
        if (lifetime <= 0)
        {
            anim.Kill();
            Destroy(gameObject);
        }

        lifetime -= Time.deltaTime;
    }
}

