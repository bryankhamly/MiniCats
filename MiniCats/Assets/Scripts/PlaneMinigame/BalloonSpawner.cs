using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject BalloonPrefab;
    public Vector2 Dir;

    public Vector2 Min;
    public Vector2 Max;

    private Vector2 _min;
    private Vector2 _max;

    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 2;

    float spawnTimer;

    private void Awake()
    {
        _min = Min;
        _max = Max;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        float rand = Random.Range(minSpawnTime, maxSpawnTime);

        if(spawnTimer >= rand)
        {
            SpawnBalloon();
        }     
    }

    void SpawnBalloon()
    {
        spawnTimer = 0;

        float randX = Random.Range(_min.x, _max.x);
        float randY = Random.Range(_min.y, _max.y);

        GameObject bal = Instantiate(BalloonPrefab, new Vector2(randX, randY), Quaternion.identity);
        Balloon b = bal.GetComponent<Balloon>();
        b.Direction = Dir;

        Minicats.instance.CurrentMinigame.MinigameObjects.Add(bal);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;

            Vector3 minPos = new Vector3(transform.position.x + _min.x, transform.position.y + _min.y, 0);
            Vector3 maxPos = new Vector3(transform.position.x + _max.x, transform.position.y + _max.y, 0);

            Gizmos.DrawSphere(minPos, 0.25f);
            Gizmos.DrawSphere(maxPos, 0.25f);

            Gizmos.DrawLine(minPos, maxPos);
        }
        else
        {
            Gizmos.color = Color.red;

            Vector3 minPos = new Vector3(transform.position.x + Min.x, transform.position.y + Min.y, 0);
            Vector3 maxPos = new Vector3(transform.position.x + Max.x, transform.position.y + Max.y, 0);

            Gizmos.DrawSphere(minPos, 0.25f);
            Gizmos.DrawSphere(maxPos, 0.25f);

            Gizmos.DrawLine(minPos, maxPos);
        }
    }
}
