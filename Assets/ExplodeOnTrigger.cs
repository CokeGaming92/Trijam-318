using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnTrigger : MonoBehaviour
{
    public GameObject explosionEffectPrefab; // Assign this in the inspector
    public AudioClip explosionSound;         // Assign this in the inspector
    public float soundVolume = 1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Instantiate explosion effect
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
            Destroy(explosion, 1f);
        }

        // Play sound at the position
        if (explosionSound != null)
        {
            GameObject tempAudioObject = new GameObject("TempAudioObject");
            tempAudioObject.transform.position = transform.position;

            AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
            tempAudioSource.clip = explosionSound;
            tempAudioSource.volume = soundVolume;
            tempAudioSource.Play();

            Destroy(tempAudioObject, explosionSound.length);
        }

        GameManager.Instance.AddScore(1);

        // Destroy the current game object
        Destroy(gameObject);
    }
}
