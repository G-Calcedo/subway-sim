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
    public float startTime;
    public float hoursDay = 24f;
    private float minutesHour = 60f;
    public float dayTimerNormalized;
    // Start is called before the first frame update
    public LightController lightSource;
    public bool isDayTime;

    void Start()
    {
        //dayTimer = Random.Range(0.0000f, 1.0000f);
        dayTimer = startTime / 24f;
    }

    // Update is called once per frame
    void Update()
    {
        dayTimer += Time.deltaTime / speed_Timer;

        dayTimerNormalized = dayTimer % 1f;

        string hours_text = Mathf.Floor(dayTimerNormalized * hoursDay).ToString("00");
        string minute_text = Mathf.Floor(((dayTimerNormalized * hoursDay) % 1f) * minutesHour).ToString("00");

        timeText.text = hours_text + " : " + minute_text;

        if(dayTimerNormalized * hoursDay < 9 && isDayTime)
        {
            lightSource.NightTransition();
            isDayTime = !isDayTime;
        }

        if (dayTimerNormalized * hoursDay >= 9f && !isDayTime)
        {
            lightSource.DayTransition();
            isDayTime = !isDayTime;
        }
    }
}
