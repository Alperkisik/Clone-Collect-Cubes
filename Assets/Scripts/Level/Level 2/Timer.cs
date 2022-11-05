using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] Level_Manager levelManager;
    [SerializeField] TextMeshProUGUI timer_textMesh;
    [SerializeField] Canvas canvas;
    [SerializeField] private int timerMinuteSetting;
    [SerializeField] private int timerSecondSetting;

    int totalSecond;
    int minute;
    int second;

    string min = "", sec = "";

    void Start()
    {
        canvas.enabled = true;
        totalSecond = timerMinuteSetting * 60 + timerSecondSetting;
        Debug.Log("Cooldown : " + totalSecond + " second");
        if (timerMinuteSetting < 10) min = "0" + minute; else min = minute.ToString();
        if (timerSecondSetting < 10) sec = "0" + second; else sec = second.ToString();

        timer_textMesh.text = timerMinuteSetting.ToString() + ":" + timerSecondSetting.ToString();
        
        listener();
        StartCoroutine(CountDown());
    }

    void listener()
    {
        Game_Manager.instance.OnLevelFailed += Event_OnLevelFailed;
        Game_Manager.instance.OnLevelSuccess += Event_OnLevelSuccess;
    }

    private void Event_OnLevelFailed(object sender, EventArgs e)
    {
        Debug.Log("cooldown stopped");
        canvas.enabled = false;
        StopAllCoroutines();
    }

    private void Event_OnLevelSuccess(object sender, EventArgs e)
    {
        Debug.Log("cooldown stopped");
        canvas.enabled = false;
        StopAllCoroutines();
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);

        minute = totalSecond / 60;
        second = totalSecond - (minute * 60);

        Debug.Log("min : " + minute + " , sec : " + second);

        totalSecond--;

        if (minute < 10) min = "0" + minute; else min = minute.ToString();
        if (second < 10) sec = "0" + second; else sec = second.ToString();

        timer_textMesh.text = min + ":" + sec;

        if (totalSecond <= 0)
        {
            Debug.Log("cooldown stopped");
            timer_textMesh.text = "00:00";
            levelManager.CountdownFinished();
            StopAllCoroutines();
        }
        else StartCoroutine(CountDown());
    }
}
