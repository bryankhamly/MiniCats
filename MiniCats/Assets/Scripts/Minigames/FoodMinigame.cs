using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

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
    public Vector3 FoodSpawnerLocation;

    public List<GameObject> FoodPrefabs;
    public List<FoodPlayerData> PlayerData;
   
    public int PointsRewardedForWinner = 25;

    public float GameTime = 45;
    float _gameTimer;

    int highscore;

    //UI
    TextMeshProUGUI FoodTimer;

    public override void Initialize()
    {
        base.Initialize();
        FoodTimer = GameObject.Find("FoodTimer").GetComponent<TextMeshProUGUI>();
        FoodInit();
    }

    void FoodInit()
    {
        FoodSpawner foodSpawner = Instantiate(FoodSpawner, FoodSpawnerLocation, Quaternion.identity).GetComponent<FoodSpawner>();

        MinigameObjects.Add(foodSpawner.gameObject);

        PlayerData = new List<FoodPlayerData>();

        int _playerCount = GameManager.instance.PlayerCount;

        for (int i = 0; i < _playerCount; i++)
        {
            FoodPlayerData newData = new FoodPlayerData();
            newData.PlayerID = i;
            newData.PlayerName = "Player " + i;
            PlayerData.Add(newData);
        }

        _gameTimer = GameTime;

        foodSpawner.start = true;

        GameManager.instance.FoodCanvas.alpha = 1;

        Debug.Log("FOODMINIGAME: INIT");
    }

    public override void Tick()
    {
        //Food Spawning Logic

        if(_playing)
        {
            _gameTimer -= Time.deltaTime;
            FoodTimer.text = ((int)_gameTimer).ToString();

            if (_gameTimer <= 0)
            {
                int winner = -1;
                CheckWinCondition(out winner);
                EndMinigame(winner, PointsRewardedForWinner);
            }

            Debug.Log("FOODMINIGAME: TICK");
        }
    }

    public override void CheckWinCondition(out int winnner)
    {
        int max = 0;
        int id = -1;

        foreach (var player in PlayerData)
        {
            int pt = player.Points;

            if(pt > max)
            {
                max = pt;
                id = player.PlayerID;
            }
        }

        highscore = max;

        winnner = id;
    }

    public override void EndMinigame(int winner, int reward)
    {
        base.EndMinigame(winner, reward);       

        if(winner != -1)
        {
            Debug.Log("Player " + winner + " won with " + highscore + " points!" + ", and has been rewarded " + reward + " points!");
        }

        GameManager.instance.FoodCanvas.alpha = 0;
    }

    public void GivePoints(int playerID, int value)
    {
        PlayerData[playerID].Points += value;
        Debug.Log("FOODMINIGAME:" + value + " POINTS GIVEN TO PLAYER" + playerID);
    }
}
