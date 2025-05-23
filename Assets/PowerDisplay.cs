using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerDisplay : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private TextMeshProUGUI _textField;

    // Start is called before the first frame update
    private void Awake()
    {
        _shipStats.OnPowerChange += UpdatePower;
    }

    private void UpdatePower(float power)
    {
        if (power > 0)
        {
            int powerInt = Mathf.RoundToInt(power);
            _textField.text = powerInt.ToString();
            _textField.color = Color.white;
        }

        else
        {
            _textField.text = "LOW";
            _textField.color = Color.red;
        }
    }

    private void OnDestroy()
    {
        _shipStats.OnPowerChange -= UpdatePower;
    }
}
