using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DodgeProjectileType
{
    Danger,
    Point
}

public class DodgeProjectile : MonoBehaviour
{
    public DodgeProjectileType _type;
    public float _speed;

    private void Awake()
    {
        Destroy(gameObject, 10);
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _speed, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_type == DodgeProjectileType.Danger)
            {
                collision.gameObject.GetComponent<DodgePlayer>().OnDanger();
                Destroy(gameObject);
            }
            else if (_type == DodgeProjectileType.Point)
            {
                if (collision.gameObject.GetComponent<DodgePlayer>().Punish)
                {
                    //Dont do anything kek
                }
                else
                {
                    collision.gameObject.GetComponent<DodgePlayer>().OnPoint();
                    Destroy(gameObject);
                }
            }
        }  
    }
}