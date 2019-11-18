using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public int ID;
    public Transform ShootPoint;
    public GameObject Projectile;

    public float _speed = 3;

    public Sprite Idle;
    public Sprite Hit;

    float down;
    float up;

    SpriteRenderer _sprite;
    bool hit;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();    
    }

    void Update()
    {
        transform.Translate(transform.right * _speed * Time.deltaTime, Space.World);

        if(down == 1)
        {
            MoveDown();
        }
        else if(up == 1)
        {
            MoveUp();
        }
    }

    public void Mad()
    {
        if(!hit)
        StartCoroutine(MadCoroutine());
    }

    IEnumerator MadCoroutine()
    {
        hit = true;
        _sprite.sprite = Hit;
        yield return new WaitForSeconds(1);
        _sprite.sprite = Idle;
        hit = false;
    }

    void ShootProjectile()
    {
        GameObject lol = Instantiate(Projectile, ShootPoint.position, ShootPoint.rotation);
        PlaneProjectile p = lol.GetComponent<PlaneProjectile>();
        p._shooterID = ID;
        GameManager.instance.CurrentMinigame.MinigameObjects.Add(lol);
    }

    public void ButtonInput(string input)
    {
        switch (input)
        {
            case "upTrue":
                up = 1;
                break;
            case "upFalse":
                up = 0;
                break;
            case "downTrue":
                down = 1;
                break;
            case "downFalse":
                down = 0;
                break;
            case "leftTrue":
                up = 1;
                break;
            case "leftFalse":
                up = 0;
                break;
            case "rightTrue":
                down = 1;
                break;
            case "rightFalse":
                down = 0;
                break;
            case "aTrue":
                ShootProjectile();
                break;
        }
    }

    void MoveDown()
    {
        transform.Rotate(new Vector3(0, 0, -2), Space.Self);
    }

    void MoveUp()
    {
        transform.Rotate(new Vector3(0, 0, 2), Space.Self);
    }
}
