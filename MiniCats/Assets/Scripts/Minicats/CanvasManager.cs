using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [Header("New Game")]
    public CanvasGroup NewGame;
    [Header("Wait For Players")]
    public CanvasGroup WaitForPlayers;
    [Header("Load Minigame")]
    public UILoadMinigame _uiLoadMinigame;
    [Header("Food Game")]
    public UIFood _uiFood;
    [Header("Plane Game")]
    public UIPlane _uiPlane;
    [Header("Scoreboard")]
    public UIDoubleScoreboard _uiScoreboard;
    [Header("GameOver")]
    public UIGameOver _uiGameOver;
    [Header("ColorBox Game")]
    public UIColorBox _uiColorBox;
    [Header("Dodge Game")]
    public UIDodge _uiDodge;

    private void Awake()
    {
        instance = this;
    }
}
