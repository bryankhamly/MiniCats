using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Game/Dodge")]
public class DodgeGame : Game
{
    public float GameTime = 60;
    float gameTimer;

    public GameObject DangerObject;
    public GameObject SushiPoint;

    public override void DistributeRewards()
    {
        CanvasManager.instance._uiDodge.MyCanvasGroup.alpha = 0;

        List<PlayerStanding> PlayerStandings = new List<PlayerStanding>();

        foreach (DodgePlayer player in Players)
        {
            PlayerStanding heh = new PlayerStanding(player.PlayerID, player.DodgePoints);
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
            GameObject p = Instantiate(player.PlayerCat.DodgeCat, Minicats.instance.GetRandomSpawnPos(), Quaternion.identity);
            ControlPlayer control = p.GetComponent<ControlPlayer>();
            Players.Add(control);
            player.CurrentControlledObject = control;
            control.InitControlPlayer(player);
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Spawn");
        List<Transform> pT = new List<Transform>();

        foreach (var item in players)
        {
            pT.Add(item.transform);
        }

        DodgeShooter[] shooters = FindObjectsOfType<DodgeShooter>();
        foreach (var item in shooters)
        {
            item.InitTargets(pT);
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        CanvasManager.instance._uiDodge.MyCanvasGroup.alpha = 1;

        gameTimer = GameTime;
        int t = (int)gameTimer;

        CanvasManager.instance._uiDodge.Timer.text = t.ToString();

        gameover = false;

        Debug.Log("[DODGE] - Init");
    }

    public override void Tick()
    {
        Debug.Log("[DODGE] - Tick");

        gameTimer -= Time.deltaTime;
        int t = (int)gameTimer;
        CanvasManager.instance._uiDodge.Timer.text = t.ToString();

        if (gameTimer <= 0 && !gameover)
        {
            Debug.Log("[DODGE] - Game Over");
            Minicats.instance.StartCoroutine(ShowScores());
            gameover = true;
        }
    }
}
