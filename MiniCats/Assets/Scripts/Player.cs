using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lightbug.Kinematic2D.Implementation;

public class Player : MonoBehaviour
{
    public int PlayerID = -1;
    public TextMeshPro NameTag;

    private CharacterHybridBrain brain;

    public GameObject PlaneObject;
    public Plane PlayerPlane;

    private void Awake()
    {
        brain = GetComponentInChildren<CharacterHybridBrain>();
    }

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

    public void ButtonInput(string input)
    {
        if(GameManager.instance.CurrentMinigame != null)
        {
            switch (GameManager.instance.CurrentMinigame.ID)
            {
                case "Food Game":
                    brain.ButtonInput(input);
                    break;
                case "Plane Game":
                    {
                        if(GameManager.instance._playingMinigame)
                        {
                            if (PlayerPlane != null)
                            {
                                PlayerPlane.ButtonInput(input);
                            }
                        }
                        else
                        {
                            brain.ButtonInput(input);
                        }
                        break;
                    }          
            }
        }
        else
        {
            brain.ButtonInput(input);
        }          
    }
}
