using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStanding
{
    public int ID;
    public int Points;

    public PlayerStanding(int _id, int _pts)
    {
        ID = _id;
        Points = _pts;
    }
}
