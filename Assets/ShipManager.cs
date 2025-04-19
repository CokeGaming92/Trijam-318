using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ShipState { Stationary, Flying, Landing, Destroyed };

public class ShipManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameWinPanel;
    [SerializeField] private GameObject _buyPanel, _sellPanel, _launchButton;
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private GameObject _shipStationary;
    [SerializeField] private GameObject _shipFlying;
    [SerializeField] private GameObject _shipLanding;
    [SerializeField] private GameObject _shipExplode;



    private bool _flying, _landed;

    // Start is called before the first frame update
    void Start()
    {
        _shipStats.GameStart();
        _shipStats.ResetShip();

        _shipStats.OnShipLaunch += LaunchShip;
        _shipStats.OnShipDestroyed += DestroyShip;
        _shipStats.OnShipDestroyed += ResetShipToStart;
        _shipStats.OnShipLanded += LandShip;
        _shipStats.OnShipLanded += ResetGameWin;

        UpdateShipState(ShipState.Stationary);
    }

    // Update is called once per frame
    void Update()
    {
        if (_flying)
            _shipStats.UpdateShip();

        if (_landed)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(DelayedRestart(0f));
            }
        }
    }

    private void UpdateShipState(ShipState state)
    {
        switch (state)
        {
            case ShipState.Stationary:
                _flying = false;
                _buyPanel.SetActive(true);
                _sellPanel.SetActive(true);
                _launchButton.SetActive(true);
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
                _landed = true;
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

    private void ResetShipToStart()
    {
        StartCoroutine(ResetShipCoroutine());
    }

    private IEnumerator ResetShipCoroutine()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds (adjust as needed)
        UpdateShipState(ShipState.Stationary);
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

    private void ResetGameWin()
    {
        _gameWinPanel.SetActive(true);
    }

    private IEnumerator DelayedRestart(float delay)
    {

        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    private void OnDestroy()
    {
        _shipStats.OnShipLaunch -= LaunchShip;
        _shipStats.OnShipDestroyed -= DestroyShip;
        _shipStats.OnShipLanded -= LandShip;
    }
}
