using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lightbug.Kinematic2D.Implementation;
using Lightbug.Kinematic2D.Core;

public class LobbyPlayer : ControlPlayer
{
    private CharacterHybridBrain brain;
    private CharacterMotor motor;

    public SpriteRenderer Sprite;
    public SpriteRenderer Icon;

    private void Awake()
    {
        brain = GetComponentInChildren<CharacterHybridBrain>();
        motor = GetComponentInChildren<CharacterMotor>();
    }

    public override void InitControlPlayer(MinicatsPlayer p)
    {
        base.InitControlPlayer(p);

        Sprite.sortingOrder = PlayerID;
        Icon.sortingOrder = PlayerID;

        Icon.color = player.PlayerColor;
    }

    public override void ReceiveInput(string input)
    {
        brain.ButtonInput(input);
    }

    public override void TeleportPlayer(Vector3 pos)
    {
        motor.Teleport(pos);
    }
}
