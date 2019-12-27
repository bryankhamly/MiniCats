using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lightbug.Kinematic2D.Implementation;
using Lightbug.Kinematic2D.Core;

public class PlanePlayer : ControlPlayer
{
    public Plane plane;
    public SpriteRenderer Sprite;
    public SpriteRenderer Icon;

    [Header("Plane")]
    public int PlanePoints = 0;

    public override void InitControlPlayer(MinicatsPlayer p)
    {
        base.InitControlPlayer(p);

        plane = GetComponentInChildren<Plane>();
        plane.ID = PlayerID;
        plane.player = this;

        Sprite.sortingOrder = PlayerID;
        Icon.sortingOrder = PlayerID;

        Icon.color = player.PlayerColor;
    }

    public override void ReceiveInput(string input)
    {
        plane.ButtonInput(input);
    }

    public override void TeleportPlayer(Vector3 pos)
    {
        transform.position = pos;
    }
}
