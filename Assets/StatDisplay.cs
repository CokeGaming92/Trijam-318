using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DisplayStat { Drain, Thrust, Weight, Firerate, Armor }
public class StatDisplay : MonoBehaviour
{
    [SerializeField] private DisplayStat _statToDisplay;
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private TextMeshProUGUI _textField;

    // Start is called before the first frame update
    void Start()
    {
        UpdateStat();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStat();
    }

    void UpdateStat()
    {
        switch (_statToDisplay)
        {
            case DisplayStat.Drain:
                _textField.text = _shipStats.PowerDrain.ToString();
                break;

            case DisplayStat.Thrust:
                int t = Mathf.RoundToInt(_shipStats.Thrust * 0.1f);
                _textField.text = t.ToString();
                break;

            case DisplayStat.Weight:
                _textField.text = _shipStats.Weight.ToString();
                break;

            case DisplayStat.Firerate:
                _textField.text = _shipStats.FireRate.ToString();
                break;

            case DisplayStat.Armor:
                int a = Mathf.RoundToInt(_shipStats.ArmorFactor * 100);
                
                _textField.text = a.ToString() + "%";
                break;

            default:
                break;

        }
        
    }
}
