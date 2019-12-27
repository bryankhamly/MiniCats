using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Game/Plane")]
public class PlaneGame : Game
{
    public float GameTime = 45;
    float gameTimer;

    [Header("Plane")]
    public int PointsPerBallon = 10;
    public int PointsPerSteal = 1;


    public override void DistributeRewards()
    {
        CanvasManager.instance._uiPlane.MyCanvasGroup.alpha = 0;

        List<PlayerStanding> PlayerStandings = new List<PlayerStanding>();

        foreach (PlanePlayer player in Players)
        {
            PlayerStanding heh = new PlayerStanding(player.PlayerID, player.PlanePoints);
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

    public override void InitPlayers()
    {
        foreach (MinicatsPlayer player in Minicats.instance.Players.Values)
        {
            GameObject p = Instantiate(player.PlayerCat.PlaneCat, Minicats.instance.GetRandomSpawnPos(), Quaternion.identity);
            ControlPlayer control = p.GetComponent<ControlPlayer>();
            Players.Add(control);
            player.CurrentControlledObject = control;
            control.InitControlPlayer(player);
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        CanvasManager.instance._uiPlane.MyCanvasGroup.alpha = 1;

        gameTimer = GameTime;

        int t = (int)gameTimer;
        CanvasManager.instance._uiPlane.Timer.text = t.ToString();

        gameover = false;

        Debug.Log("[PLANE] - Init");
    }

    public override void Tick()
    {
        Debug.Log("[PLANE] - Tick");

        gameTimer -= Time.deltaTime;
        int t = (int)gameTimer;
        CanvasManager.instance._uiPlane.Timer.text = t.ToString();

        if (gameTimer <= 0 && !gameover)
        {
            Debug.Log("[PLANE GAME] - Game Over");
            Minicats.instance.StartCoroutine(ShowScores());
            gameover = true;
        }
    }

    public void RewardPoints(PlanePlayer player)
    {
        player.PlanePoints += PointsPerBallon;
    }

    public void StealPoints(int victim, int shooter)
    {
        PlanePlayer victimPlane = (PlanePlayer)Minicats.instance.CurrentMinigame.Players[victim];
        PlanePlayer shooterPlane = (PlanePlayer)Minicats.instance.CurrentMinigame.Players[shooter];

        victimPlane.PlanePoints -= PointsPerSteal;
        shooterPlane.PlanePoints += PointsPerSteal;

        victimPlane.plane.Mad();
    }
}
