using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Game : ScriptableObject
{
    [Header("Game Info")]
    public string ID;
    public string Description;

    [Header("Game Data")]
    public GameObject MapPrefab;
    public int Reward;

    [Header("Game Temp Data")]
    public List<GameObject> MinigameObjects;
    public List<ControlPlayer> Players;

    protected bool IsPlaying { get => Minicats.instance.Gamestate == Gamestate.MINIGAME; }

    public abstract void InitPlayers();
    public abstract void DistributeRewards();

    protected bool gameover = false;

    public virtual void Initialize()
    {
        CleanUpLobbyCats();

        MinigameObjects = new List<GameObject>();
        Players = new List<ControlPlayer>();

        InitMap();
        InitPlayers();
    }

    void CleanUpLobbyCats()
    {
        foreach (var item in Minicats.instance.LobbyCats)
        {
            Destroy(item);
        }

        Minicats.instance.LobbyCats.Clear();
    }

    public virtual void InitMap()
    {
        GameObject map = Instantiate(MapPrefab, Vector3.zero, Quaternion.identity);
        MinigameObjects.Add(map);
    }

    public virtual void Cleanup()
    {
        CleanUpMinigameObjects();
        CleanupPlayers();
    }

    public virtual void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Minicats.instance.StartCoroutine(ShowScores());
        }
    }

    public virtual IEnumerator ShowScores()
    {
        //Clean up
        Cleanup();
        //Reward

        DistributeRewards();

        CanvasManager.instance._uiScoreboard.MyCanvasGroup.alpha = 1;
        CanvasManager.instance._uiScoreboard.GetActualResults();

        Minicats.instance.RoundsLeft--;

        float t = 10;

        while (t > 0)
        {
            Debug.Log("Showing Winner for " + t + " seconds.");
            yield return new WaitForSeconds(1);
            t--;
        }

        CanvasManager.instance._uiScoreboard.MyCanvasGroup.alpha = 0;
        EndGame();
    }

    public virtual void EndGame()
    {
        Minicats.instance.Gamestate = Gamestate.LOBBY;     
        Minicats.instance.SpawnLobbyMap();
        Minicats.instance.SpawnLobbyCats();
        gameover = false;
    }

    public virtual void CleanUpMinigameObjects()
    {
        if (MinigameObjects.Count > 0)
        {
            Debug.Log("Cleaning up [" + MinigameObjects.Count + "] minigame object(s).");

            foreach (var item in MinigameObjects)
            {
                Destroy(item);
            }
        }
    }

    public virtual void CleanupPlayers()
    {
        foreach (var p in Minicats.instance.Players.Values)
        {
            p.CurrentControlledObject = null;
        }

        if (Players.Count > 0)
        {
            Debug.Log("Cleaning up [" + Players.Count + "] player(s).");

            foreach (var item in Players)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
        }
    }
}
