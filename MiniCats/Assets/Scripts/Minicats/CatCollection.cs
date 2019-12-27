using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cat Collection")]
public class CatCollection : ScriptableObject
{
    public List<Cats> Cats = new List<Cats>();
}
