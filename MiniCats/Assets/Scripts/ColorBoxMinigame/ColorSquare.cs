using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSquare : MonoBehaviour
{
    public SpriteRenderer Sprite;
    public Color CurrentColor = Color.white;

    private void Awake()
    {
        Sprite = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color c)
    {
        Sprite.color = c;
        CurrentColor = c;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<ControlPlayer>())
        {
            ChangeColor(collision.gameObject.GetComponentInParent<ControlPlayer>().PlayerColor);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<ControlPlayer>())
        {
            ChangeColor(collision.gameObject.GetComponentInParent<ControlPlayer>().PlayerColor);
        }
    }
}
