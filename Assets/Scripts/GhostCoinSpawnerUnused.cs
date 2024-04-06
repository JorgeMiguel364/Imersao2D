using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostCoinSpawnerUnused : MonoBehaviour
{
    public GameObject GoldCoin;
    public GameObject Ghost1;

    public Tilemap mainTile;

    private float spawnTime1 = 4f;
    private float spawnTime2 = 8f;

    private List<Vector3> validSpawnPositions = new List<Vector3>();

    void Start()
    {
        GatherValidPositions();

        InvokeRepeating("SpawnCoins", spawnTime1, spawnTime1);
        InvokeRepeating("SpawnGhosts", spawnTime2, spawnTime2);
    }

    // Cria as moedas no cenário
    private void SpawnCoins()
    {
        if (validSpawnPositions.Count == 0)
            return;

        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        while (!validPositionFound && validSpawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            Vector3 potentialPosition = validSpawnPositions[randomIndex];

            validPositionFound = true;
            spawnPosition = potentialPosition;
            validSpawnPositions.RemoveAt(randomIndex);
        }

        if (validPositionFound && GameController.instance.coinsQt < GameController.instance.maxCoins)
        {
            Instantiate(GoldCoin, spawnPosition, Quaternion.identity);
        }
    }

    // Cria os inimigos no cenário
    private void SpawnGhosts()
    {
        if (validSpawnPositions.Count == 0)
            return;

        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        while (!validPositionFound && validSpawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            Vector3 potentialPosition = validSpawnPositions[randomIndex];

            validPositionFound = true;
            spawnPosition = potentialPosition;
            validSpawnPositions.RemoveAt(randomIndex);
        }

        if (validPositionFound && GameController.instance.enemiesQt < GameController.instance.maxEnemies)
        {
            Instantiate(Ghost1, spawnPosition, Quaternion.identity);
        }
    }

    // Captura todas as posições válidas no cenário
    private void GatherValidPositions()
    {
        validSpawnPositions.Clear();

        BoundsInt boundsInt = mainTile.cellBounds;
        TileBase[] allTiles = mainTile.GetTilesBlock(boundsInt);
        Vector3 start = mainTile.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));

        for (int x = 0; x < boundsInt.size.x; x++)
        {
            for (int y = 0; y < boundsInt.size.y; y++)
            {
                TileBase tile = allTiles[(x + y * boundsInt.size.x)];

                if (tile == null)
                {
                    Vector3 place = start + new Vector3(x + .5f, y + .5f, 0);
                    validSpawnPositions.Add(place);
                }
            }
        }
    }
}
