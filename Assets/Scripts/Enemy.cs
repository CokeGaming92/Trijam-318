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

    private void Start()
    {
        _shipStats.OnShipLanded += KillAlien;
    }


    public void KillAlien()
    {
        _scoreObject.value += _scoreValue;
        Destroy(gameObject);
    }


    private void Update()
    {
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
    }
}
