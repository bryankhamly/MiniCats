using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int PlayerID = -1;
    public TextMeshPro NameTag;

    public void SetPlayerID(int id)
    {
        PlayerID = id;
        NameTag.text = "P" + PlayerID;
        GetComponentInChildren<SpriteRenderer>().sortingOrder = id;
    }
}
