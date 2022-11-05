using System;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [SerializeField] List<GameObject> Levels;

    public static Game_Manager instance;

    public event EventHandler OnLevelFailed;
    public event EventHandler OnLevelSuccess;
    public event EventHandler OnNextLevel;
    public event EventHandler OnRetryLevel;
    public event EventHandler OnLevelStarted;
    public event EventHandler OnLevelLoaded;

    [HideInInspector] public bool IslevelFinished;
    [HideInInspector] public bool IslevelStarted;

    int levelIndex = 0;
    GameObject level;

    private void Awake()
    {
        instance = this;
        IslevelFinished = false;
    }

    private void Start()
    {
        InstantiateLevel(levelIndex);
    }

    public void LevelSuccess()
    {
        IslevelFinished = true;
        IslevelStarted = false;
        OnLevelSuccess?.Invoke(this, EventArgs.Empty);
    }

    public void LevelFailed()
    {
        IslevelFinished = true;
        IslevelStarted = false;
        OnLevelFailed?.Invoke(this, EventArgs.Empty);
    }

    public void StartLevel()
    {
        IslevelStarted = true;
        OnLevelStarted?.Invoke(this, EventArgs.Empty);
    }

    private void DestroyCurrentLevel()
    {
        Destroy(level);
    }

    private void InstantiateLevel(int levelIndex)
    {
        IslevelFinished = false;
        IslevelStarted = false;
        level = Instantiate(Levels[levelIndex], Vector3.zero, Quaternion.identity);
        OnLevelLoaded?.Invoke(this, EventArgs.Empty);
        StartLevel();
    }

    public void NextLevel()
    {
        levelIndex++;
        if (levelIndex >= Levels.Count) levelIndex = 0;
        DestroyCurrentLevel();
        InstantiateLevel(levelIndex);
    }

    public void RetryLevel()
    {
        DestroyCurrentLevel();
        InstantiateLevel(levelIndex);
    }
}
