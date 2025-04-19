using UnityEngine;
using System.Collections.Generic;

public class TurretController : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private string enemyTag = "Enemy";      // Tag for enemies to track
    [SerializeField] private float range = 50f;              // Tracking range
    [SerializeField] private float secondsBetweenShots = 30f;           // Shots per second
    [SerializeField] private float lineDuration = 0.2f;     // How long the line persists (seconds)

    [SerializeField] private AudioClip _gunSound;
    [SerializeField] private AudioSource _audioSource;

    private float nextFireTime;
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private LineRenderer lineRenderer;

    void Start()
    {
        // Initialize LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // Update enemies in range
        UpdateEnemiesInRange();

        // Find closest enemy
        GameObject closestEnemy = GetClosestEnemy();

        // Shoot if ready
        if (closestEnemy != null && Time.time >= nextFireTime)
        {
            Shoot(closestEnemy);
            nextFireTime = Time.time + (secondsBetweenShots / _shipStats.FireRate);
        }
    }

    void UpdateEnemiesInRange()
    {
        // Clear null enemies
        enemiesInRange.RemoveAll(enemy => enemy == null);

        // Find all enemies with tag
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // Update list of enemies in range
        enemiesInRange.Clear();
        foreach (GameObject enemy in allEnemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= range)
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    GameObject GetClosestEnemy()
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in enemiesInRange)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    void Shoot(GameObject target)
    {
        // Draw line to target
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
        lineRenderer.enabled = true;

        // Call KillAlien on target
        var enemyScript = target.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.KillAlien();
        }
        else
        {
            Debug.LogWarning("Target enemy missing Enemy script!");
        }

        // Play sound
        _audioSource.PlayOneShot(_gunSound);

        // Disable line after duration
        StartCoroutine(DisableLineAfterDelay());
    }

    System.Collections.IEnumerator DisableLineAfterDelay()
    {
        yield return new WaitForSeconds(lineDuration);
        lineRenderer.enabled = false;
    }
}