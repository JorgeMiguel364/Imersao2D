using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCoinSpawner : MonoBehaviour
{
    public GameObject GoldCoin;
    public GameObject Ghost1;

    private float secondsMin = 1f;
    private float secondsMax = 60f;

    void Start()
    {
        InvokeRepeating("spawnCoins", Random.Range(secondsMin, secondsMax), Random.Range(secondsMin, secondsMax));
        InvokeRepeating("spawnGhosts", Random.Range(secondsMin, secondsMax), Random.Range(secondsMin, secondsMax));
    }

    private void spawnCoins()
    {
        Renderer rend = GetComponent<Renderer>();

        Vector2 spawnPoint = new Vector2(Random.Range(rend.bounds.min.x, rend.bounds.max.x), Random.Range(rend.bounds.min.y, rend.bounds.max.y));

        if (GameController.instance.coinsQt < GameController.instance.maxCoins)
        {
            Instantiate(GoldCoin, spawnPoint, Quaternion.identity);
        }
    }

    private void spawnGhosts()
    {
        Renderer rend = GetComponent<Renderer>();

        Vector2 spawnPoint = new Vector2(Random.Range(rend.bounds.min.x, rend.bounds.max.x), Random.Range(rend.bounds.min.y, rend.bounds.max.y));

        if (GameController.instance.enemiesQt < GameController.instance.maxEnemies)
        {
            Instantiate(Ghost1, spawnPoint, Quaternion.identity);
        }
    }
}
