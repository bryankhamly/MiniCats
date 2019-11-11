using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodPlayerData
{
    public string PlayerName;
    public int PlayerID;
    public int Points;
}

[CreateAssetMenu(fileName = "New Food Minigame")]
public class FoodMinigame : Minigame
{
    public GameObject FoodSpawner;
    public List<GameObject> FoodPrefabs;
    public List<FoodPlayerData> PlayerData;
    public int PointsNeededToWin = 10;
    public int PointsRewardedForWinner = 25;

    public override void Initialize()
    {
        base.Initialize();

        PlayerData = new List<FoodPlayerData>();

        int _playerCount = GameManager.instance.PlayerCount;

        for (int i = 0; i < _playerCount; i++)
        {
            FoodPlayerData newData = new FoodPlayerData();
            newData.PlayerID = i;
            newData.PlayerName = "Player " + i;
            PlayerData.Add(newData);
        }
    }

    public override void Tick()
    {
        //Food Spawning Logic
        Debug.Log("FOODMINIGAME: TICK");
    }

    public override void CheckWinCondition()
    {
        foreach (var item in PlayerData)
        {
            if(item.Points == PointsNeededToWin)
            {
                EndMinigame(item.PlayerID);
            }
        }
    }

    public override void EndMinigame(int playerID)
    {
        Debug.Log("Player " + playerID + " won, and was rewarded: " + PointsRewardedForWinner + " points!");
        GameManager.instance.PlayerData[playerID].Points += PointsRewardedForWinner;
        base.EndMinigame(playerID);
    }

    public void GivePoints(int playerID, int value)
    {
        PlayerData[playerID].Points += value;
        CheckWinCondition();
    }
}
