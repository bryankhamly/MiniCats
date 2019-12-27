using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeShooter : MonoBehaviour
{
    public float Min = 0.5f;
    public float Max = 1.0f;

    public List<Transform> Targets;

    float pewTimer = 0;

    public void InitTargets(List<Transform> targets)
    {
        Targets = new List<Transform>(targets);
    }

    void Update()
    {
        pewTimer += Time.deltaTime;

        if(pewTimer >= Random.Range(Min,Max))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        pewTimer = 0;

        int rand = Random.Range(0, 2);
        DodgeGame dodge = Minicats.instance.CurrentMinigame as DodgeGame;

        if (rand == 0)
        {
            GameObject kek = Instantiate(dodge.DangerObject, transform.position, Quaternion.identity);
            dodge.MinigameObjects.Add(kek);

            Vector2 dir = Targets[Random.Range(0, Targets.Count)].position - kek.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            kek.transform.rotation = rot;
        }
        else
        {
            GameObject kek = Instantiate(dodge.SushiPoint, transform.position, Quaternion.identity);
            dodge.MinigameObjects.Add(kek);

            Vector2 dir = Targets[Random.Range(0, Targets.Count)].position - kek.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            kek.transform.rotation = rot;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 1);
    }
}
