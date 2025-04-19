using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private IntVariable _scoreObject;
    [SerializeField] private float _speed;
    [SerializeField] private string _playerTag;
    [SerializeField] private float _damageAmount;
    [SerializeField] private int _scoreValue;

    private bool _moving;

    private void Start()
    {
        _shipStats.OnShipLanded += KillAlien;
        _shipStats.OnShipDestroyed += FreezeAlien;

        _moving = true;
    }


    public void KillAlien()
    {
        _scoreObject.value += _scoreValue;
        _shipStats.AddCurrency(_scoreValue);
        Destroy(gameObject);
    }


    public void FreezeAlien()
    {
        _moving = false;
    }

    private void Update()
    {
        if (!_moving)
            return;

        Vector2 dir = transform.position.normalized * -_speed * Time.deltaTime; // send it towards the center of the screen

        transform.Translate(dir.x, dir.y, 0);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.CompareTag(_playerTag))
        {
            _shipStats.TakeDamage(_damageAmount);
            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        _shipStats.OnShipLanded -= KillAlien;
        _shipStats.OnShipDestroyed -= FreezeAlien;
    }
}
