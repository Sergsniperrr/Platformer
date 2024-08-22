using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;

    private readonly int _nomberOfCoins = 20;

    private void Start()
    {
        List<Vector2> randomPoints = CreateRandomPoints();

        foreach (Vector2 point in randomPoints)
        {
            Instantiate(_coinPrefab, point, Quaternion.identity);
        }
    }

    private List<Vector2> CreateAllPoins()
    {
        List<Vector2> allCoinsPoints = new();
        Vector2 offset = new(-15.5f, -8.5f);
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    allCoinsPoints.Add(new Vector2(x, y) + offset);
                }
            }
        }

        return allCoinsPoints;
    }

    private List<Vector2> CreateRandomPoints()
    {
        List<Vector2> allPoints = CreateAllPoins();
        List<Vector2> randomPoints = new();
        Vector2 point;

        if (_nomberOfCoins >= allPoints.Count)
            return allPoints;

        for (int i = 0; i < _nomberOfCoins; i++)
        {
            do
            {
                point = allPoints[Random.Range(0, allPoints.Count)];
            }
            while (randomPoints.Contains(point));

            randomPoints.Add(point);
        }

        return randomPoints;
    }


}