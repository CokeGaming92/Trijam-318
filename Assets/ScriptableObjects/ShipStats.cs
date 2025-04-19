using UnityEngine;
using System;
using System.Collections;

public enum StatType { Engine, Battery, Gun, Armor };

[CreateAssetMenu(menuName = "ShipStats")]
public class ShipStats : ScriptableObject
{
    private float _powerTotal;
    private float _powerDrain;
    private float _weight;
    private float _thrust;
    private float _fireRate;
    private float _armorFactor;
    private float _currency;

    private float _distanceToGo;

    private int _engineCount;
    private int _batteryCount;
    private int _gunCount;
    private int _armorCount;

    private float _timeOfLastUpdate;

    public float Power => _powerTotal;
    public float PowerDrain => _powerDrain;
    public float Weight => _weight;
    public float Thrust => _thrust;
    public float FireRate => _fireRate;
    public float ArmorFactor => _armorFactor;
    public float Currency => _currency;


    // Events for when stuff changes
    public event Action<float> OnPowerChange; // Event to notify when score changes
    public event Action<float> OnDistanceChange; // Event to notify when score changes
    public event Action<float> OnCurrencyChange; // Event to notify when currency changes

    // One unified event to update price UI based on StatType
    public event Action<StatType, int> OnPriceChange;

    public event Action OnShipLaunch;
    public event Action OnShipLanded;
    public event Action OnShipDestroyed;

    public void GameStart()
    {
        _currency = 1000f;

        _engineCount = 1;
        _batteryCount = 1;
        _gunCount = 1;
        _armorCount = 0;
        _distanceToGo = 384400;

        UpdateStats();
    }
    public void ResetShip()
    {
        _distanceToGo = 384400;
        UpdateStats();
    }

    //
    // Here is where the stats can be balanced
    //
    private void UpdateStats()
    {
        _powerTotal = _batteryCount * 100; // each battery gives 100 power
        _powerDrain = (_engineCount * 3) + (_gunCount * 3); //_armorCount; // each engine drains 2 power per second, guns drain 1
        _weight = (_batteryCount*12) + (_engineCount*5) + (_gunCount*2) + (_armorCount * 3); // each module is 1 weight, armor is 2
        _thrust = (_engineCount * 500) - (_weight * 1.8f); // each engine can support 3 modules
        _fireRate = _gunCount * 0.5f;  // firerate goes up by 1 per gun
        _armorFactor = Mathf.Clamp(_armorCount * 0.02f, 0, 0.95f); // damage reduced by 5% for each armor

        _currency = Mathf.Max(_currency, Mathf.Epsilon);

        OnDistanceChange?.Invoke(_distanceToGo);
        OnPowerChange?.Invoke(_powerTotal);
        OnCurrencyChange?.Invoke(_currency);

        // Trigger price updates for each stat individually
        OnPriceChange?.Invoke(StatType.Engine, GetNextStatPrice(StatType.Engine));
        OnPriceChange?.Invoke(StatType.Battery, GetNextStatPrice(StatType.Battery));
        OnPriceChange?.Invoke(StatType.Gun, GetNextStatPrice(StatType.Gun));
        OnPriceChange?.Invoke(StatType.Armor, GetNextStatPrice(StatType.Armor));

    }

    public int GetNextStatPrice(StatType stat)
    {
        switch (stat)
        {
            case StatType.Engine: return _engineCount + 1;
            case StatType.Battery: return _batteryCount + 1;
            case StatType.Gun: return _gunCount + 1;
            case StatType.Armor: return _armorCount + 1;
            default: return 0;
        }
    }

    // This starts the power drain on the ship, basically starts the timer
    public void LaunchShip()
    {
        _timeOfLastUpdate = Time.time;

        OnShipLaunch?.Invoke();
    }

    // This will be called by the ship manager script, hopefully once per frame
    public void UpdateShip()
    {
        float timePassed = Time.time - _timeOfLastUpdate; // find out how many seconds since last update

        //
        // Do distance
        //
        _distanceToGo -= _thrust * timePassed;

        if (_distanceToGo <= 0)
            ShipLanded();

        OnDistanceChange?.Invoke(_distanceToGo);


        //
        // Do power drain
        //
        _powerTotal -= (_powerDrain * timePassed);
        _timeOfLastUpdate = Time.time;

        if (_powerTotal < 0)
            ShipDestroyed();

        OnPowerChange?.Invoke(_powerTotal);

        _timeOfLastUpdate = Time.time;
    }

    public void TakeDamage(float damageAmount)
    {
        _powerTotal -= damageAmount - (damageAmount * _armorFactor);
        OnPowerChange?.Invoke(_powerTotal);
    }

    public void Abort()
    {
        ShipDestroyed();
    }

    private void ShipDestroyed()
    {
        _powerTotal = 0;

        OnPowerChange?.Invoke(_powerTotal);
        OnShipDestroyed?.Invoke();
    }

    private void ShipLanded()
    {
        _distanceToGo = 0;
        OnShipLanded?.Invoke();
    }

    public void AddCurrency(int howMuch)
    {
        _currency += howMuch;
        OnCurrencyChange?.Invoke(_currency);

    }

    public void AddShipStat(StatType stat)
    {
        int price = GetNextStatPrice(stat);
        if (_currency < price) return;

        switch (stat)
        {
            case StatType.Engine: _engineCount++; break;
            case StatType.Battery: _batteryCount++; break;
            case StatType.Gun: _gunCount++; break;
            case StatType.Armor: _armorCount++; break;
        }

        _currency -= price;
        UpdateStats();
    }


    public void RemoveShipStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.Engine:
                if (_engineCount > 0)
                {
                    _engineCount--;
                    _currency += _engineCount + 1;
                }
                break;
            case StatType.Battery:
                if (_batteryCount > 0)
                {
                    _batteryCount--;
                    _currency += _batteryCount + 1;
                }
                break;
            case StatType.Gun:
                if (_gunCount > 0)
                {
                    _gunCount--;
                    _currency += _gunCount + 1;
                }
                break;
            case StatType.Armor:
                if (_armorCount > 0)
                {
                    _armorCount--;
                    _currency += _armorCount + 1;
                }
                break;
        }

        UpdateStats();
    }

    public int ReadShipStat(StatType stat)
    {
        return stat switch
        {
            StatType.Engine => _engineCount,
            StatType.Battery => _batteryCount,
            StatType.Gun => _gunCount,
            StatType.Armor => _armorCount,
            _ => 0
        };
    }

  
}