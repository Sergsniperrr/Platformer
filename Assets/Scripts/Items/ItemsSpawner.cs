using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private HealthPoint _healthPointPrefab;
    [SerializeField] private Tilemap _coinsMap;
    [SerializeField] private int _numberOfSpawnCoins = 20;
    [SerializeField] private int _numberOfSpawnHealthPoints = 5;

    private void Start()
    {
        List<Vector2> allPoints = CreateAllPoins();

        List<Vector2> coins = CreateRandomPoints(allPoints, _numberOfSpawnCoins);
        List<Vector2> healthPoints = CreateRandomPoints(allPoints, _numberOfSpawnHealthPoints);

        foreach (Vector2 point in coins)
            Instantiate(_coinPrefab, point, Quaternion.identity);

        foreach (Vector2 point in healthPoints)
            Instantiate(_healthPointPrefab, point, Quaternion.identity);
    }

    private List<Vector2> CreateAllPoins()
    {
        List<Vector2> allItemsPoints = new();
        Vector2 offset = new(-15.5f, -8.5f);
        BoundsInt bounds = _coinsMap.cellBounds;
        TileBase[] allTiles = _coinsMap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    allItemsPoints.Add(new Vector2(x, y) + offset);
                }
            }
        }

        return allItemsPoints;
    }

    private List<Vector2> CreateRandomPoints(List<Vector2> allPoints, int numberOfSpawnItems)
    {
        List<Vector2> randomPoints = new();
        Vector2 point;

        if (numberOfSpawnItems >= allPoints.Count)
            return allPoints;

        for (int i = 0; i < numberOfSpawnItems; i++)
        {
            point = allPoints[Random.Range(0, allPoints.Count)];

            randomPoints.Add(point);
            allPoints.Remove(point);
        }

        return randomPoints;
    }
}