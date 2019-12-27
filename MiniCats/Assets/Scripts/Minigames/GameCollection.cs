using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Collection")]
public class GameCollection : ScriptableObject
{
    public List<Game> Games = new List<Game>();
}
