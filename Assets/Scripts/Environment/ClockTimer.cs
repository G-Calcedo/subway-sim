using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockTimer : MonoBehaviour
{
    //Segundos reales equivalentes a 24 horas en el juego
    public float speed_Timer;
    public Text timeText;
    private float dayTimer;
    private float hoursDay = 24f;
    private float minutesHour = 60f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dayTimer += Time.deltaTime / speed_Timer;

        float dayTimerNormalized = dayTimer % 1f;

        string hours_text = Mathf.Floor(dayTimerNormalized * hoursDay).ToString("00");
        string minute_text = Mathf.Floor(((dayTimerNormalized * hoursDay) % 1f) * minutesHour).ToString("00");

        timeText.text = hours_text + " : " + minute_text;

    }
}
