using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : ScriptableObject
{
    public string ID;
    public string Description;
    public int StartTime = 5;

    public List<GameObject> MinigameObjects;
  
    public abstract void Tick();
    public abstract void CheckWinCondition(out int winner);

    protected bool _playing;

    public virtual void Initialize()
    {
        MinigameObjects = new List<GameObject>();
        _playing = true;
        GameManager.instance.ShowWinner(false);
        GameManager.instance.ShowNametags(false);
    }

    public virtual void EndMinigame(int winner, int reward)
    {
        _playing = false;
        CleanUpMinigameObjects();

        if(winner == -1)
        {
            Debug.Log("NO ONE WON XD");
        }
        else
        {
            GameManager.instance.PlayerData[winner].Points += reward;
        }
        
        GameManager.instance.StopMinigame();
        GameManager.instance.ShowWinner(true, winner);
        GameManager.instance.ShowNametags(true);
        GameManager.instance.ReduceRounds();

        Debug.Log("Minigame End: " + ID);
    }

    public virtual void CleanUpMinigameObjects()
    {
        if(MinigameObjects.Count > 0)
        {
            Debug.Log("Cleaning up [" + MinigameObjects.Count + "] minigame object(s).");

            foreach (var item in MinigameObjects)
            {
                Destroy(item);
            }
        }
    }
}
