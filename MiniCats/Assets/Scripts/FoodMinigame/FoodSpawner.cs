using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public bool start;

    [Header("Spawn Distance")]
    public float _minX = -5;
    public float _maxX = 5;

    [Header("Spawn Settings")]
    public float _minSpawnTime;
    public float _maxSpawnTime;

    float _spawnTimer = 0;

    private void Update()
    {
        if (start)
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= Random.Range(_minSpawnTime, _maxSpawnTime))
            {
                SpawnFood();
            }
        }

    }

    void SpawnFood()
    {
        _spawnTimer = 0;
        FoodMinigame food = GameManager.instance.CurrentMinigame as FoodMinigame;

        int rand = Random.Range(0, food.FoodPrefabs.Count - 1);
        GameObject randFood = food.FoodPrefabs[rand];

        GameObject newFood = Instantiate(randFood, GetRandomSpawnPoint(), Quaternion.identity);
        GameManager.instance.CurrentMinigame.MinigameObjects.Add(newFood);
    }

    Vector2 GetRandomSpawnPoint()
    {
        float random = Random.Range(_minX, _maxX);
        return new Vector2(random, transform.position.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 minPos = new Vector3(transform.position.x + _minX, transform.position.y, 0);
        Vector3 maxPos = new Vector3(transform.position.x + _maxX, transform.position.y, 0);

        Gizmos.DrawSphere(minPos, 0.25f);
        Gizmos.DrawSphere(maxPos, 0.25f);

        Gizmos.DrawLine(minPos, maxPos);
    }
}
