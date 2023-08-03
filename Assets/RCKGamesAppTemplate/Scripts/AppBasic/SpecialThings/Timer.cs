using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    [Header("**Time Variables**")]
    private DateTime eventDate;
    private DateTime todayDate;
    private TimeSpan timeSpan;

    public GameObject timerContainer;
    public Text daysLeft;
    public Text hours;
    public Text minutes;
    public Text seconds;


    private void OnEnable()
    {
        eventDate = DateTime.Parse("5/25/2019 10:00:00 AM"); // "mes/dia/año hora:minutos:segundos AM o PM"
        todayDate = DateTime.Now;
        timeSpan = todayDate - eventDate;
        // Debug.Log ("Today Date " + todayDate);
        // Debug.Log ("Event Date " + eventDate);
        // Debug.Log ("Time Span " + timeSpan);
        // Debug.Log ("span total hours " + timeSpan.TotalHours);
        // Debug.Log(eventDate.DayOfYear);
    }

    void Update()
    {
        if (timerContainer.activeInHierarchy)
        {
            Timer_();
        }
    }

    void Timer_()
    {
        timeSpan = eventDate - DateTime.Now;

        if (eventDate > DateTime.Now)
        {
            daysLeft.text = (eventDate.DayOfYear - DateTime.Now.DayOfYear).ToString() + " Días";

            //string timeToStartEvent = String.Concat(timeSpan.Hours.ToString("00"), ":", timeSpan.Minutes.ToString("00"), ":", timeSpan.Seconds.ToString("00"));
            //hours.text = timeToStartEvent;
            hours.text = timeSpan.TotalHours.ToString("## 'horas'");
            minutes.text = timeSpan.TotalMinutes.ToString("## 'minutos'");
            seconds.text = timeSpan.TotalSeconds.ToString("## 'segundos'");
        }
    }

}
