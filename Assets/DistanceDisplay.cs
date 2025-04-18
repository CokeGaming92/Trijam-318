using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceDisplay : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private TextMeshProUGUI _textField;

    // Start is called before the first frame update
    private void Awake()
    {
        _shipStats.OnDistanceChange += UpdateDistance;
    }

    private void UpdateDistance(float distance)
    {
        int dist = Mathf.RoundToInt(distance);
        _textField.text = dist.ToString() + " km";
    }

    private void OnDestroy()
    {
        _shipStats.OnDistanceChange -= UpdateDistance;
    }
}
