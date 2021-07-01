using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnstile : MonoBehaviour
{
    public GameObject leftDoor, rightDoor;

    private Vector3 leftClosed, leftOpen, rightClosed, rightOpen;

    public GameObject entryPoint;

    private void Awake()
    {
        leftClosed = leftDoor.transform.localPosition;
        leftOpen = leftClosed + new Vector3(-1, 0, 0);
        rightClosed = rightDoor.transform.localPosition;
        rightOpen = rightClosed + new Vector3(1, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        OpenAndCloseAnimation();
    }

    public void OpenAndCloseAnimation()
    {
        leftDoor.transform.DOLocalMove(leftOpen, 0.5f).OnComplete(() =>
        {
            leftDoor.transform.DOLocalMove(leftClosed, 0.5f);
        });

        rightDoor.transform.DOLocalMove(rightOpen, 0.5f).OnComplete(() =>
        {
            rightDoor.transform.DOLocalMove(rightClosed, 0.5f);
        });
    }
}
