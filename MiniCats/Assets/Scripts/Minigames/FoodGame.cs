using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Game/Food")]
public class FoodGame : Game
{
    [Header("Food Settings")]
    public GameObject FoodSpawnerPrefab;
    public Vector3 FoodSpawnerLocation;
    public List<GameObject> FoodPrefabs;
    public float GameTime = 45;

    float gameTimer;

    public override void InitPlayers()
    {
        foreach (MinicatsPlayer player in Minicats.instance.Players.Values)
        {
            GameObject p = Instantiate(player.PlayerCat.FoodCat, Minicats.instance.GetRandomSpawnPos(), Quaternion.identity);
            ControlPlayer control = p.GetComponent<ControlPlayer>();
            Players.Add(control);
            player.CurrentControlledObject = control;
            control.InitControlPlayer(player);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        CanvasManager.instance._uiFood.MyCanvasGroup.alpha = 1;

        FoodSpawner foodSpawner = Instantiate(FoodSpawnerPrefab, FoodSpawnerLocation, Quaternion.identity).GetComponent<FoodSpawner>();
        MinigameObjects.Add(foodSpawner.gameObject);

        foodSpawner.start = true;

        gameTimer = GameTime;

        int t = (int)gameTimer;
        CanvasManager.instance._uiFood.Timer.text = t.ToString();

        gameover = false;

        Debug.Log("[FOOD] - Init");
    }

    public override void Tick()
    {
        Debug.Log("[FOOD] - Tick");

        gameTimer -= Time.deltaTime;
        int t = (int)gameTimer;
        CanvasManager.instance._uiFood.Timer.text = t.ToString();

        if (gameTimer <= 0 && !gameover)
        {
            Debug.Log("[FOOD GAME] - Game Over");
            Minicats.instance.StartCoroutine(ShowScores());
            gameover = true;   
        }
    }

    public void GivePoints(FoodPlayer player, int value)
    {
        player.FoodPoints += value;
    }

    public override void DistributeRewards()
    {
        CanvasManager.instance._uiFood.MyCanvasGroup.alpha = 0;

        List<PlayerStanding> PlayerStandings = new List<PlayerStanding>();

        foreach (FoodPlayer player in Players)
        {
            PlayerStanding heh = new PlayerStanding(player.PlayerID, player.FoodPoints);
            PlayerStandings.Add(heh);
        }

        PlayerStanding[] sorted = PlayerStandings.OrderByDescending(c => c.Points).ToArray();

        CanvasManager.instance._uiScoreboard.GetGameResults(sorted, Reward);

        //1st Place - Gets full points.
        Minicats.instance.RewardPlayer(sorted[0].ID, Reward);

        int splitReward = Reward;

        for (int i = 1; i < sorted.Length; i++)
        {
            splitReward = splitReward / 2;

            if (splitReward < 2)
            {
                //No more rewards.
                Minicats.instance.RewardPlayer(sorted[i].ID, 1);
            }
            else
            {
                Minicats.instance.RewardPlayer(sorted[i].ID, splitReward);
            }            
        }     
    }
}
