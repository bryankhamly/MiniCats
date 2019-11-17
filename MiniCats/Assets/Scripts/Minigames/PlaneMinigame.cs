﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

[System.Serializable]
public class PlanePlayerData
{
    public int PlayerID;
    public int Points;
}

[CreateAssetMenu(fileName = "New Plane Minigame")]
public class PlaneMinigame : Minigame
{
    public int PointsRewardedForWinner = 25;

    public float GameTime = 45;
    float _gameTimer;

    int highscore;

    public List<PlanePlayerData> PlayerData;

    TextMeshProUGUI PlaneTimer;
    TextMeshProUGUI PlayerListPoints;

    string scores;

    public override void Initialize()
    {
        base.Initialize();

        PlayerData = new List<PlanePlayerData>();

        int _playerCount = GameManager.instance.PlayerCount;

        for (int i = 0; i < _playerCount; i++)
        {
            PlanePlayerData newData = new PlanePlayerData();
            newData.PlayerID = i;
            PlayerData.Add(newData);
        }

        for (int i = 0; i < GameManager.instance.PlayerList.Count; i++)
        {
            GameObject lol = Instantiate(GameManager.instance.PlayerList[i].PlaneObject, Vector2.zero, Quaternion.identity);
            MinigameObjects.Add(lol);

            Plane heh = lol.GetComponent<Plane>();
            GameManager.instance.PlayerList[i].PlayerPlane = heh;
        }

        PlaneTimer = GameObject.Find("PlaneTimer").GetComponent<TextMeshProUGUI>();
        PlayerListPoints = GameObject.Find("PlayerListPoints").GetComponent<TextMeshProUGUI>();

        _gameTimer = GameTime;
        GameManager.instance.PlaneCanvas.alpha = 1;

        UpdateScores();
    }

    void UpdateScores()
    {
        scores = "";

        foreach (var item in PlayerData)
        {
            scores += "Player " + item.PlayerID + ": " + item.Points + "\n";
        }

        PlayerListPoints.text = scores;
    }

    public override void Tick()
    {
        if (_playing)
        {
            _gameTimer -= Time.deltaTime;
            PlaneTimer.text = ((int)_gameTimer).ToString();

            if (_gameTimer <= 0)
            {
                int winner = -1;
                CheckWinCondition(out winner);
                EndMinigame(winner, PointsRewardedForWinner);
            }
        }
    }

    public override void CheckWinCondition(out int winnner)
    {
        winnner = 1;
    }

    public override void EndMinigame(int winner, int reward)
    {
        foreach (var item in GameManager.instance.PlayerList)
        {
            item.PlayerPlane = null;
        }

        base.EndMinigame(winner, reward);

        GameManager.instance.PlaneCanvas.alpha = 0;
    }
}