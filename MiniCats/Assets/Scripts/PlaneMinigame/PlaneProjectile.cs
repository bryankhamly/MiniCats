using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneProjectile : MonoBehaviour
{
    public int _shooterID;
    public float _speed;
    public PlaneMinigame PlaneGame;

    void Start()
    {
        StartCoroutine(Bye());
    }

    IEnumerator Bye()
    {
        yield return new WaitForSeconds(3);
        GameManager.instance.CurrentMinigame.MinigameObjects.Remove(gameObject);
        Destroy(gameObject);
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _speed, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plane"))
        {
            Plane p = collision.gameObject.GetComponent<Plane>();
            if (p.ID == _shooterID)
                return;

            //Steal
            PlaneGame.StealPoints(p.ID, _shooterID);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Balloon"))
        {
            //Reward
            collision.gameObject.GetComponent<Balloon>().Pop(_shooterID);
            Destroy(gameObject);
        }
    }
}
