using TMPro;
using UnityEngine;

public class Price : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private TextMeshProUGUI _displayText;
    [SerializeField] private StatType _statType; // Set in inspector

    private void Awake()
    {
        _shipStats.OnPriceChange += UpdatePriceUI;
    }

    private void OnDestroy()
    {
        _shipStats.OnPriceChange -= UpdatePriceUI;
    }

    private void UpdatePriceUI(StatType stat, int currentPrice)
    {
        if (stat == _statType)
            _displayText.text = $"${currentPrice}";
    }
}
