using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner1 : MonoBehaviour
{
    public GameObject Ghost;

    private float spawnTime = 8f;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn() 
    {
        if (GameController.instance.enemiesQt < GameController.instance.maxEnemies)
        {
            Renderer rend = GetComponent<Renderer>();

            float x1 = transform.position.x - rend.bounds.size.x / 2;
            float x2 = transform.position.x + rend.bounds.size.x / 2;

            Vector2 spawnPoint = new Vector2(Random.Range(x1, x2), transform.position.y);

            Instantiate(Ghost, spawnPoint, Quaternion.identity);
        }
    }
}
