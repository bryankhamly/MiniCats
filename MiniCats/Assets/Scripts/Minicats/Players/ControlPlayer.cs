using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlPlayer : MonoBehaviour
{
    protected MinicatsPlayer player;

    public virtual void InitControlPlayer(MinicatsPlayer p)
    {
        player = p;
    }

    public abstract void ReceiveInput(string input);
    public abstract void TeleportPlayer(Vector3 pos);

    public virtual int PlayerID
    {
        get => player.PlayerID;
    }

    public virtual Color PlayerColor
    {
        get => player.PlayerColor;
    }
}
