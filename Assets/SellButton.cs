using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SellButton : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private TextMeshProUGUI _displayText;
    [SerializeField] private StatType _statToDisplay;

    // Start is called before the first frame update

    private void Update()
    {
        _displayText.text = _shipStats.ReadShipStat(_statToDisplay).ToString();
    }

    public void Sell()
    {
        _shipStats.RemoveShipStat(_statToDisplay);
    }

}
