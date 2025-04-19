using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private TextMeshProUGUI _textField;

    private void Awake()
    {
        _shipStats.OnCurrencyChange += UpdateCurrencyUI;
    }
  
    public void UpdateCurrencyUI(float currency)
    {
        int curr = Mathf.RoundToInt(currency);
        _textField.text = "$" + curr.ToString();

        
    }

    private void OnDestroy()
    {
        _shipStats.OnCurrencyChange -= UpdateCurrencyUI;
    }
}
