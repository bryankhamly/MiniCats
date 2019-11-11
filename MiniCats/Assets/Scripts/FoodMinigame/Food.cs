using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodMinigame _foodMinigame;

    public int PointValue = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponentInParent<Player>())
        {
            int id = collision.collider.GetComponentInParent<Player>().PlayerID;
            _foodMinigame.GivePoints(id, PointValue);
            _foodMinigame.MinigameObjects.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
