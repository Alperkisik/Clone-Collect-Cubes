using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_UI_Manager : MonoBehaviour
{
    public static Game_UI_Manager instance;

    [SerializeField] GameObject successed;
    [SerializeField] GameObject failed;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Listener();
    }

    private void Listener()
    {
        Game_Manager.instance.OnLevelFailed += Event_OnLevelFailed;
        Game_Manager.instance.OnLevelSuccess += Event_OnLevelSuccess;
        Game_Manager.instance.OnLevelLoaded += Event_OnLevelLoaded;
        Game_Manager.instance.OnLevelStarted += Event_OnLevelStarted;
    }

    private void Event_OnLevelStarted(object sender, System.EventArgs e)
    {

    }

    private void Event_OnLevelLoaded(object sender, System.EventArgs e)
    {

    }

    private void Event_OnLevelSuccess(object sender, System.EventArgs e)
    {
        SetActive_LevelEndUI(true, false);
    }

    private void Event_OnLevelFailed(object sender, System.EventArgs e)
    {
        SetActive_LevelEndUI(false, true);
    }

    public void NextLevel()
    {
        SetActive_LevelEndUI(false, false);
        Game_Manager.instance.NextLevel();
    }

    public void RetryLevel()
    {
        SetActive_LevelEndUI(false, false);
        Game_Manager.instance.RetryLevel();
    }

    private void SetActive_LevelEndUI(bool success, bool fail)
    {
        successed.SetActive(success);
        failed.SetActive(fail);
    }
}
