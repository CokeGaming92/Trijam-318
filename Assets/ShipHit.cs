using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHit : MonoBehaviour
{
    [SerializeField] private Animator _shipAnimator;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _shipAnimator.SetTrigger("Hit");
    }
}
