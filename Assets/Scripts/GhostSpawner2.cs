using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner2 : MonoBehaviour
{
    public GameObject Ghost;

    private float spawnTime = 5f;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        if (GameController.instance.enemiesQt < GameController.instance.maxEnemies)
        {
            Renderer rend = GetComponent<Renderer>();

            float y1 = transform.position.y - rend.bounds.size.y / 2;
            float y2 = transform.position.y + rend.bounds.size.y / 2;

            Vector2 spawnPoint = new Vector2(transform.position.x, Random.Range(y1, y2));

            Instantiate(Ghost, spawnPoint, Quaternion.identity);
        }
    }

}
