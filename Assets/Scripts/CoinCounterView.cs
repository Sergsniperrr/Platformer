using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class CoinCounterView : MonoBehaviour
{
    [SerializeField] private Score _coins;

    private TMP_Text _counterText;

    private void OnEnable()
    {
        _coins.NumberOfCoinsChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _coins.NumberOfCoinsChanged -= OnValueChanged;
    }

    private void Start()
    {
        _counterText = GetComponent<TMP_Text>();
    }

    private void OnValueChanged(int currentNumberOfCoins)
    {
        _counterText.text = currentNumberOfCoins.ToString();
    }
}
