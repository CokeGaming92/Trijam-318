using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ShipStats _shipStats;
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private float _initialSpawnDelay;
    [SerializeField] private float _spawnsPerMinute;
    [SerializeField] private float offsetDistance = 1f; // Distance outside screen edges to spawn

    private float _startTime;
    private float _nextSpawnTime;
    private bool _spawning;

    private void Start()
    {
        _shipStats.OnShipLaunch += StartSpawning;
        _shipStats.OnShipLanded += StopSpawning;
        _shipStats.OnShipDestroyed += StopSpawning;

        _spawning = false;
    }


    private void Update()
    {
        if ( _spawning)
        {
            if (_nextSpawnTime <= Time.time)
            {
                Vector3 spawnPosition = GetRandomOffScreenPoint();
                int i = Random.Range(0, _enemyPrefabs.Count);

                Instantiate(_enemyPrefabs[i], spawnPosition, Quaternion.identity);

                _nextSpawnTime = Time.time + (60 / (_spawnsPerMinute + (_shipStats.Thrust * 0.005f)));
            }
        }
    }


    private void StartSpawning()
    {
        _nextSpawnTime = Time.time + _initialSpawnDelay;// Time.time + (60 / (_spawnsPerMinute + (_shipStats.Thrust*0.5f)));
        _spawning = true;
    }

    private void StopSpawning()
    {
        _spawning = false;
    }

    private void OnDestroy()
    {
        _shipStats.OnShipLaunch -= StartSpawning;
        _shipStats.OnShipLanded -= StopSpawning;
        _shipStats.OnShipDestroyed -= StopSpawning;
    }

    public Vector3 GetRandomOffScreenPoint(Camera camera = null)
    {
        // Use main camera if none provided
        if (camera == null)
        {
            camera = Camera.main;
        }

        if (camera == null)
        {
            Debug.LogError("No camera found!");
            return Vector2.zero;
        }

        // Get screen bounds in world space
        float aspect = (float)Screen.width / Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        float cameraWidth = cameraHeight * aspect;

        Vector2 cameraPos = camera.transform.position;
        float leftEdge = cameraPos.x - cameraWidth / 2;
        float rightEdge = cameraPos.x + cameraWidth / 2;
        float topEdge = cameraPos.y + cameraHeight / 2;
        float bottomEdge = cameraPos.y - cameraHeight / 2;

        // Pick a random edge (0: left, 1: right, 2: top, 3: bottom)
        int edge = Random.Range(0, 4);

        Vector2 point;
        switch (edge)
        {
            case 0: // Left edge
                point = new Vector2(leftEdge - offsetDistance, Random.Range(bottomEdge, topEdge));
                break;
            case 1: // Right edge
                point = new Vector2(rightEdge + offsetDistance, Random.Range(bottomEdge, topEdge));
                break;
            case 2: // Top edge
                point = new Vector2(Random.Range(leftEdge, rightEdge), topEdge + offsetDistance);
                break;
            case 3: // Bottom edge
                point = new Vector2(Random.Range(leftEdge, rightEdge), bottomEdge - offsetDistance);
                break;
            default:
                point = Vector2.zero;
                break;
        }

        return point;
    }
}
