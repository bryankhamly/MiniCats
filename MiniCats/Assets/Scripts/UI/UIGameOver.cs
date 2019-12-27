using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UIGameOver : MonoBehaviour
{
    public CanvasGroup MyCanvasGroup;
    public TextMeshProUGUI ActualResults;
    public TextMeshProUGUI RestartTimer;

    public bool GotResults = false;

    public void GetResults()
    {
        GotResults = true;

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

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
}
