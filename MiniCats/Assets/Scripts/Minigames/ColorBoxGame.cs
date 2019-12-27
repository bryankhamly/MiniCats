using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Game/ColorBox")]
public class ColorBoxGame : Game
{
    public float GameTime = 60;

    public List<ColorSquare> Squares;

    float gameTimer;

    public override void DistributeRewards()
    {
        CanvasManager.instance._uiColorBox.MyCanvasGroup.alpha = 0;
        List<PlayerStanding> PlayerStandings = new List<PlayerStanding>();

        foreach (ColorBoxPlayer player in Players)
        {
            int pts = 0;

            foreach (var item in Squares)
            {
                if (item.CurrentColor == player.PlayerColor)
                {
                    pts++;
                }
            }

            PlayerStanding heh = new PlayerStanding(player.PlayerID, pts);
            PlayerStandings.Add(heh);

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

    public override void InitPlayers()
    {
        foreach (MinicatsPlayer player in Minicats.instance.Players.Values)
        {
            GameObject p = Instantiate(player.PlayerCat.ColorBoxCat, Minicats.instance.GetRandomSpawnPos(), Quaternion.identity);
            ControlPlayer control = p.GetComponent<ControlPlayer>();
            Players.Add(control);
            player.CurrentControlledObject = control;
            control.InitControlPlayer(player);
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        CanvasManager.instance._uiColorBox.MyCanvasGroup.alpha = 1;

        ColorSquare[] dankSquares = FindObjectsOfType<ColorSquare>();
        Squares = new List<ColorSquare>();

        foreach (ColorSquare dank in dankSquares)
        {
            Squares.Add(dank);
        }

        gameTimer = GameTime;
        int t = (int)gameTimer;

        CanvasManager.instance._uiColorBox.Timer.text = t.ToString();

        gameover = false;

        Debug.Log("[COLOR BOX] - Init");
    }

    public override void Tick()
    {
        Debug.Log("[COLOR BOX] - Tick");

        gameTimer -= Time.deltaTime;
        int t = (int)gameTimer;
        CanvasManager.instance._uiColorBox.Timer.text = t.ToString();

        if (gameTimer <= 0 && !gameover)
        {
            Debug.Log("[COLOR BOX] - Game Over");
            Minicats.instance.StartCoroutine(ShowScores());
            gameover = true;
        }
    }
}
