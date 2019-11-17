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
        NameTag.text = id.ToString();
        GetComponentInChildren<SpriteRenderer>().sortingOrder = id;
    }

    public void ShowNametag(bool yes)
    {
        if(yes)
        {
            NameTag.gameObject.SetActive(true);
        }
        else
        {
            NameTag.gameObject.SetActive(true);
        }
    }
}
