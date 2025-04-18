using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    [SerializeField] private StatType _statToBuy;
    [SerializeField] private ShipStats _shipStats;

    public void Buy()
    {
        _shipStats.AddShipStat(_statToBuy);
    }
}
