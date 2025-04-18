using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StatType {Engine, Battery, Gun, Armor};

[CreateAssetMenu(menuName = "ShipStats")]
public class ShipStats : ScriptableObject
{
    private float _powerTotal;
    private float _powerDrain;
    private float _weight;
    private float _thrust;
    private float _fireRate;
    private float _armorFactor;

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


    // Events for when stuff changes
    public event Action<float> OnPowerChange; // Event to notify when score changes
    public event Action<float> OnDistanceChange; // Event to notify when score changes

    public event Action OnShipLaunch;
    public event Action OnShipLanded;
    public event Action OnShipDestroyed;

    public void ResetShip()
    {
        _engineCount = 5;
        _batteryCount = 5;
        _gunCount = 1;
        _armorCount = 0;
        _distanceToGo = 384400;

        UpdateStats();
    }

    //
    // Here is where the stats can be balanced
    //
    private void UpdateStats()
    {
        _powerTotal = _batteryCount * 100; // each battery gives 100 power
        _powerDrain = (_engineCount * 2) + (_gunCount * 1); // each engine drains 2 power per second, guns drain 1
        _weight = (_batteryCount) + (_engineCount) + (_gunCount) + (_armorCount * 2); // each module is 1 weight, armor is 2
        _thrust = (_engineCount * 1000) - (_weight); // each engine can support 3 modules
        _fireRate = _gunCount;  // firerate goes up by 1 per gun
        _armorFactor = _armorCount * 0.05f; // damage reduced by 5% for each armor

        OnDistanceChange?.Invoke(_distanceToGo);
        OnPowerChange?.Invoke(_powerTotal);

        
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


    private void ShipDestroyed()
    {
        _powerTotal = 0;
        OnShipDestroyed?.Invoke();
    }

    private void ShipLanded()
    {
        _distanceToGo = 0;
        OnShipLanded?.Invoke();
    }



    public void AddShipStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.Engine:
                _engineCount++;
                break;

            case StatType.Battery:
                _batteryCount++;
                break;

            case StatType.Gun:
                _gunCount++;
                break;

            case StatType.Armor:
                _armorCount++;
                break;

            default:
                break;
        }

        UpdateStats();
    }

    public void RemoveShipStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.Engine:
                if ( _engineCount > 0 ) _engineCount--;
                break;

            case StatType.Battery:
                if (_batteryCount > 0) _batteryCount--;
                break;

            case StatType.Gun:
                if (_gunCount > 0) _gunCount--;
                break;

            case StatType.Armor:
                if (_armorCount > 0) _armorCount--;
                break;

            default:
                break;
        }

        UpdateStats();
    }

    public int ReadShipStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.Engine:
                return _engineCount;

            case StatType.Battery:
                return _batteryCount;

            case StatType.Gun:
                return _gunCount;

            case StatType.Armor:
                return _armorCount;

            default:
                return 0;
        }
    }
}