using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HealthTextView : HealthView
{
    private TextMeshProUGUI _text;
    private string _separator = "/";

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = Health.MaxValue + _separator + Health.MaxValue;
    }

    protected override void ChangeValue(int newValue)
    {
        _text.text = newValue + _separator + Health.MaxValue;
    }
}
