using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinicatsPlayer : MonoBehaviour
{
    [Header("Player Data")]
    public int DeviceID = -1;
    public int PlayerID = -1;
    public Color PlayerColor;
    public int Points = 0;
    public Cats PlayerCat;

    [Header("Game Data")]
    public ControlPlayer CurrentControlledObject;

    public void InitializePlayer(int newID, Color newColor, Cats cat, int device)
    {
        PlayerID = newID;
        PlayerColor = newColor;
        Points = 0;
        PlayerCat = cat;
        DeviceID = device;

        gameObject.name = "Minicats Player " + PlayerID;
    }

    public void SendInput(string input)
    {
        if (CurrentControlledObject != null)
            CurrentControlledObject.ReceiveInput(input);
    }
}
