using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private float _starsSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _shipStats.OnShipLaunch += StartStars;
        _shipStats.OnShipLanded += StopStars;
        _shipStats.OnShipDestroyed += StopStars;
    }

    private void StartStars()
    {
        var main = _particles.main;
        main.gravityModifier = _starsSpeed;

        ParticleSystem.EmissionModule emission = _particles.emission;

        emission.rateOverTime = 100; // Emit 100 particles per second
    }

    private void StopStars()
    {
        var main = _particles.main;
        main.gravityModifier = 0;

        ParticleSystem.EmissionModule emission = _particles.emission;

        emission.rateOverTime = 30; // Emit 100 particles per second
    }

    private void OnDestroy()
    {
        _shipStats.OnShipLaunch -= StartStars;
        _shipStats.OnShipLanded -= StopStars;
        _shipStats.OnShipDestroyed -= StopStars;
    }
}
