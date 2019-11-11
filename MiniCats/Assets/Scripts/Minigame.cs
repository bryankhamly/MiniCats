using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : ScriptableObject
{
    public string ID;
    public string Description;

    public List<GameObject> MinigameObjects;
  
    public abstract void Tick();
    public abstract void CheckWinCondition();

    public virtual void Initialize()
    {
        MinigameObjects = new List<GameObject>();
    }

    public virtual void EndMinigame(int playerID)
    {
        CleanUpMinigameObjects();
        GameManager.instance.StopMinigame();
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
