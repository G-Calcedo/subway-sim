using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightController : MonoBehaviour
{
    public Camera cameraSource;
    private Light lightSource;

    private Color dayColor;
    private Color nightColor;

    private Color dayBGColor;
    private Color nightBGColor;

    private void Awake()
    {
        lightSource = GetComponent<Light>();

        dayColor = lightSource.color;
        nightColor = new Color(0f / 255f, 26f / 255f, 38f / 255f);

        dayBGColor = cameraSource.backgroundColor;
        nightBGColor = new Color(37f / 255f, 40f / 255f, 80f / 255f);
    }

    public void NightTransition()
    {
        lightSource.DOColor(nightColor, 5);
        cameraSource.DOColor(nightBGColor, 5);
    }

    public void DayTransition()
    {
        lightSource.DOColor(dayColor, 5);
        cameraSource.DOColor(dayBGColor, 5);
    }
}
