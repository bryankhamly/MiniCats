using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodGame _foodMinigame;

    public int PointValue = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponentInParent<ControlPlayer>())
        {
            FoodPlayer player = collision.collider.GetComponent<FoodPlayer>();
            _foodMinigame.GivePoints(player, PointValue);
            _foodMinigame.MinigameObjects.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
