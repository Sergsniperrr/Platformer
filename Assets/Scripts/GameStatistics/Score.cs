using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int _collectedCoins;

    public event Action<int> NumberOfCoinsChanged;

    public void IncreaseNumberOfCoin()
    {
        _collectedCoins ++;

        NumberOfCoinsChanged?.Invoke(_collectedCoins);
    }
}
