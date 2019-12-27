using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lightbug.Kinematic2D.Implementation;
using Lightbug.Kinematic2D.Core;

public class DodgePlayer : ControlPlayer
{
    public SpriteRenderer Sprite;
    public SpriteRenderer Icon;

    [Header("Dodge")]
    public int DodgePoints = 0;
    public float _speed;
    public Vector2 _dir;

    [Header("Sprites")]
    public Sprite Idle;
    public Sprite Hit;

    public bool Punish;

    public override void InitControlPlayer(MinicatsPlayer p)
    {
        base.InitControlPlayer(p);
        Sprite.sortingOrder = PlayerID;
        Icon.sortingOrder = PlayerID;
        Icon.color = player.PlayerColor;
    }

    public void OnDanger()
    {
        if (!Punish)
            StartCoroutine(MadCoroutine());
    }

    IEnumerator MadCoroutine()
    {
        Punish = true;
        Sprite.sprite = Hit;
        yield return new WaitForSeconds(2);
        Sprite.sprite = Idle;
        Punish = false;
    }

    public void OnPoint()
    {
        DodgePoints += 10;
    }

    public override void ReceiveInput(string input)
    {
        switch (input)
        {
            case "upTrue":
                _dir = new Vector2(_dir.x, 1);
                break;
            case "upFalse":
                _dir = new Vector2(_dir.x, 0);
                break;
            case "downTrue":
                _dir = new Vector2(_dir.x, -1);
                break;
            case "downFalse":
                _dir = new Vector2(_dir.x, 0);
                break;
            case "leftTrue":
                _dir = new Vector2(-1, _dir.y);
                break;
            case "leftFalse":
                _dir = new Vector2(0, _dir.y);
                break;
            case "rightTrue":
                _dir = new Vector2(1, _dir.y);
                break;
            case "rightFalse":
                _dir = new Vector2(0, _dir.y);
                break;
        }
    }

    private void Update()
    {
        transform.Translate(_dir * _speed * Time.deltaTime, Space.World);
    }

    public override void TeleportPlayer(Vector3 pos)
    {
        transform.position = pos;
    }
}
