using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UIDoubleScoreboard : MonoBehaviour
{
    public CanvasGroup MyCanvasGroup;
    [Header("Minigame")]
    public TextMeshProUGUI MinigameTitleText;
    public TextMeshProUGUI MinigameResults;
    [Header("Actual")]
    public TextMeshProUGUI ActualResults;

    public void GetActualResults()
    {
        string result = string.Empty;

        List<PlayerStanding> PlayerStandings = new List<PlayerStanding>();

        foreach (var player in Minicats.instance.Players)
        {
            PlayerStanding heh = new PlayerStanding(player.Value.PlayerID, player.Value.Points);
            PlayerStandings.Add(heh);
        }

        PlayerStanding[] sorted = PlayerStandings.OrderByDescending(c => c.Points).ToArray();       

        for (int i = 0; i < sorted.Length; i++)
        {
            string colorHex = ColorToHex(Minicats.instance.GetPlayerByID(sorted[i].ID).PlayerColor);       
            result += string.Format("<color=#{0}>Player {1} ({2})</color> \n", colorHex, sorted[i].ID, sorted[i].Points);
        }

        ActualResults.text = result;
    }

    public void GetGameResults(PlayerStanding[] standings, int reward)
    {
        string result = string.Empty;

        MinigameTitleText.text = Minicats.instance.CurrentMinigame.ID + " Results";

        int splitReward = reward;

        //Player 0: 999 (+100)
        for (int i = 0; i < standings.Length; i++)
        {
            string colorHex = ColorToHex(Minicats.instance.GetPlayerByID(standings[i].ID).PlayerColor);
            result += string.Format("<color=#{0}>Player {1}: [{2}] - (+{3})</color> \n", colorHex, standings[i].ID, standings[i].Points, splitReward);

            splitReward = splitReward / 2;

            if(splitReward < 2)
            {
                splitReward = 1;
            }
        }

        MinigameResults.text = result;
    }

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
}
