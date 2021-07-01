using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musician : MonoBehaviour
{
    public GameObject walk, playing, hat;

    private StateMachineEngine musicianSM;

    private void Start()
    {
        musicianSM = new StateMachineEngine();
        Perception moneyReceived = musicianSM.CreatePerception<PushPerception>();
        Perception keepPlaying = musicianSM.CreatePerception<TimerPerception>(0.5f);

        Tween musicAnim = null;

        State playingMusic = musicianSM.CreateEntryState("Playing", () =>
        {
            keepPlaying.Reset();
            walk.SetActive(false);
            playing.SetActive(true);
            hat.SetActive(true);
            Debug.Log("Playing");
            playing.transform.DOScale(200, 0.15f).OnComplete(() =>
            {
                musicAnim = playing.transform.DOScale(250, 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash);
            });
        });

        State thankingMoney = musicianSM.CreateState("Thanking", () =>
        {
            Debug.Log("GRACIAS AMIGO");
            musicAnim.Kill();
            playing.transform.DOLocalJump(playing.transform.localPosition, 2, 1, 0.5f);
        });

        musicianSM.CreateTransition("Thank", playingMusic, moneyReceived, thankingMoney);
        musicianSM.CreateTransition("Play", thankingMoney, keepPlaying, playingMusic);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            musicianSM.Fire("Thank");
        }

        musicianSM.Update();
    }
}
