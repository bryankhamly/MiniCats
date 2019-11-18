using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float Speed;
    public Vector2 Direction;
    public PlaneMinigame PlaneGame;

    void Update()
    {
        transform.Translate(Direction * Speed * Time.deltaTime);
    }

    public void Pop(int id)
    {
        PlaneGame.RewardPoints(id);
        Destroy(gameObject);
    }
}
