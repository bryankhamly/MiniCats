using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SendMessageUpwards("TeleportPlayer", ResetPos, SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SendMessageUpwards("TeleportPlayer", ResetPos, SendMessageOptions.DontRequireReceiver);
    }

    Vector3 ResetPos
    {
        get
        {
            return GameObject.FindGameObjectWithTag("Reset").transform.position;
        }
    }
}
