using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int _collectedCoins;

    public event Action<int> NumberOfCoinsChanged;

    public void IncreaseNomberOfCoin()
    {
        NumberOfCoinsChanged?.Invoke(++_collectedCoins);
    }
}
