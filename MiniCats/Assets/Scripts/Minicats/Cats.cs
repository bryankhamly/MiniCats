using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cat")]
public class Cats : ScriptableObject
{
    public string CatID;
    public GameObject LobbyCat;
    public GameObject FoodCat;
    public GameObject PlaneCat;
    public GameObject ColorBoxCat;
    public GameObject DodgeCat;
}
