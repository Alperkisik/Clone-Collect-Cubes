using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    public static Level_Manager instance;

    int opponentScore, playerScore;

    public event EventHandler OnLevelEnded;
    public event EventHandler OnPlayerGetScore;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Listener();

        opponentScore = 0;
        playerScore = 0;
    }

    private void Listener()
    {
        Collectable_Manager.instance.OnAllCubesCollected += Event_OnAllCubesCollected;
    }

    public void Trigger_LevelEndEvent()
    {
        OnLevelEnded?.Invoke(this, EventArgs.Empty);
    }

    private void Event_OnAllCubesCollected(object sender, EventArgs e)
    {
        Trigger_LevelEndEvent();

        if (playerScore > opponentScore) LevelSuccess();
        else if (playerScore <= opponentScore) LevelFailed();
    }

    public void IncreasePlayerScore(int value)
    {
        playerScore += value;
        OnPlayerGetScore?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseOpponentScore(int value)
    {
        opponentScore += value;
    }

    private void LevelSuccess()
    {
        Game_Manager.instance.LevelSuccess();
    }

    private void LevelFailed()
    {
        Game_Manager.instance.LevelFailed();
    }

    public void CountdownFinished()
    {
        Trigger_LevelEndEvent();
        //Game_Manager.instance.LevelFailed();
    }
}
