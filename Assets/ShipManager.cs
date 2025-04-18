using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipState { Stationary, Flying, Landing, Destroyed};

public class ShipManager : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private GameObject _shipStationary;
    [SerializeField] private GameObject _shipFlying;
    [SerializeField] private GameObject _shipLanding;
    [SerializeField] private GameObject _shipExplode;

    private bool _flying;

    // Start is called before the first frame update
    void Start()
    {
        _shipStats.ResetShip();

        _shipStats.OnShipLaunch += LaunchShip;
        _shipStats.OnShipDestroyed += DestroyShip;
        _shipStats.OnShipLanded += LandShip;

        UpdateShipState(ShipState.Stationary);
    }

    // Update is called once per frame
    void Update()
    {
        if (_flying)
            _shipStats.UpdateShip();
    }

    private void UpdateShipState(ShipState state)
    {
        switch (state)
        {
            case ShipState.Stationary:
                _flying = false;
                _shipStationary.SetActive(true);
                _shipFlying.SetActive(false);
                _shipLanding.SetActive(false);
                _shipExplode.SetActive(false);
                break;

            case ShipState.Flying:
                _flying = true;
                _shipStationary.SetActive(false);
                _shipFlying.SetActive(true);
                _shipLanding.SetActive(false);
                _shipExplode.SetActive(false);
                break;

            case ShipState.Landing:
                _flying = false;
                _shipStationary.SetActive(false);
                _shipFlying.SetActive(false);
                _shipLanding.SetActive(true);
                _shipExplode.SetActive(false);
                break;

            case ShipState.Destroyed:
                _flying = false;
                _shipStationary.SetActive(false);
                _shipFlying.SetActive(false);
                _shipLanding.SetActive(false);
                _shipExplode.SetActive(true);
                break;

            default:
                break;
        }
    }


    private void LaunchShip()
    {
        UpdateShipState(ShipState.Flying);
    }

    private void LandShip()
    {
        UpdateShipState(ShipState.Landing);
    }

    private void DestroyShip()
    {
        UpdateShipState(ShipState.Destroyed);
    }


    private void OnDestroy()
    {
        _shipStats.OnShipLaunch -= LaunchShip;
        _shipStats.OnShipDestroyed -= DestroyShip;
        _shipStats.OnShipLanded -= LandShip;
    }
}
