using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerColors")]
public class PlayerColors : ScriptableObject
{
    public List<Color> Colors = new List<Color>();
}
