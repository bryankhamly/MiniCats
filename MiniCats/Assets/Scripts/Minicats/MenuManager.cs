using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("New Game UI")]
    public TextMeshProUGUI RoundsToPlay;
    public Outline UpButton;
    public Outline DownButton;
    public Outline PlayButton;

    private List<Outline> buttons = new List<Outline>();
    private int buttonIndex = 0;

    void Start()
    {
        instance = this;

        buttons.Add(UpButton);
        buttons.Add(DownButton);
        buttons.Add(PlayButton);

        foreach (var item in buttons)
        {
            item.enabled = false;
        }

        ChangeSelection(0);
    }

    public void ShowNewGame(bool show)
    {
        if(show)
        {
            CanvasManager.instance.NewGame.alpha = 1;
        }
        else
        {
            CanvasManager.instance.NewGame.alpha = 0;
        }
    }

    public void GetInput(string input)
    {
        switch (input)
        {
            case "rightTrue":
                ChangeSelection(buttonIndex + 1); 
                break;
            case "leftTrue":
                ChangeSelection(buttonIndex - 1);
                break;
            case "upTrue":
                ChangeSelection(buttonIndex - 1);
                break;
            case "downTrue":
                ChangeSelection(buttonIndex + 1);
                break;
            case "aTrue":
                DoAction();
                break;
        }
    }

    public void ChangeSelection(int index)
    {
        int num = index;

        if (num < 0)
            num = 2;

        if (num >= 3)
            num = 0;

        buttons[buttonIndex].enabled = false;
        buttonIndex = num;
        buttons[buttonIndex].enabled = true;
    }

    public void DoAction()
    {
        if (Minicats.instance.Gamestate == Gamestate.NEWGAME)
        {
            switch (buttonIndex)
            {
                case 0:
                    Minicats.instance.RoundsToPlay++;
                    RoundsToPlay.text = Minicats.instance.RoundsToPlay.ToString();
                    break;
                case 1:
                    Minicats.instance.RoundsToPlay--;

                    if (Minicats.instance.RoundsToPlay <= 0)
                    {
                        Minicats.instance.RoundsToPlay = 0;
                    }

                    RoundsToPlay.text = Minicats.instance.RoundsToPlay.ToString();
                    break;
                case 2:
                    Debug.Log("[NEWGAME] - Start");
                    if(Minicats.instance.RoundsToPlay == 0)
                    {
                        Minicats.instance.RoundsToPlay = 1;
                    }
                    Minicats.instance.StartGame();
                    break;
            }
        }
    }
}
