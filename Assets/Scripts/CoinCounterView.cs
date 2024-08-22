using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class CoinCounterView : MonoBehaviour
{
    private TMP_Text _counterText;

    private void Start()
    {
        _counterText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (_counterText.text != Coin.TotalCoins.ToString())
            _counterText.text = Coin.TotalCoins.ToString();
    }
}
