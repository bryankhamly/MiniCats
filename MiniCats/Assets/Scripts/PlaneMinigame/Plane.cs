using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public float _speed = 3;

    float down;
    float up;

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
