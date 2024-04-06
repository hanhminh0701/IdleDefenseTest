using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] Transform[] _groundSpawnPoints, _skySpawnPoints, _groundPath;
    [SerializeField] SpawningPattern[] _phases;
    [SerializeField] Transform _enemyPool, _bulletPool;

    [SerializeField] GroundEnemy _groundEnemyPrefab;
    [SerializeField] SkyEnemy _skyEnemyPrefab;
    [SerializeField] Transform _canvas;

    [HideInInspector] public Queue<GroundEnemy> GroundEnemies = new();
    [HideInInspector] public Queue<SkyEnemy> SkyEnemies = new();
    [HideInInspector] public Queue<BulletController> Bullets = new();

    public event Action ON_GAME_OVER;
    public Transform[] GroundSpawnPoints => _groundSpawnPoints;
    public Transform[] SkySpawnPoints => _skySpawnPoints;
    public Transform[] GroundPath => _groundPath;
    public Transform Canvas => _canvas;
    public Transform BulletPool => _bulletPool;

    float _timeToSpawn;
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        SpawnEnemy(_timeToSpawn);
        _timeToSpawn += Time.deltaTime;
    }

    void SpawnEnemy(float time)
    {
        var phase = _phases.FirstOrDefault(p => p.TimeToSpawn == time);
        phase?.SpawnEnemy();
    }

    public GroundEnemy GetGroundEnemy(Vector2 position)
    {
        GroundEnemy enemy;
        if (GroundEnemies.Count > 0) enemy = GroundEnemies.Dequeue();
        else enemy = Instantiate(_groundEnemyPrefab, _enemyPool);
        enemy.Respawn(position);
        return enemy;
    }
    public SkyEnemy GetSkyEnemy(Vector2 position)
    {
        SkyEnemy enemy;
        if (GroundEnemies.Count > 0) enemy = SkyEnemies.Dequeue();
        else enemy = Instantiate(_skyEnemyPrefab, _enemyPool);
        enemy.Respawn(position);
        return enemy;
    }

    public void ResetSpawnTime() => _timeToSpawn = 0;

    public void FireGameOver()
    {
        ON_GAME_OVER?.Invoke();
    }
}

[Serializable]
public class SpawningPattern
{
    public float TimeToSpawn;
    public EnemyType Type;
    public int Amount;

    public void SpawnEnemy()
    {
        int spawnPointIndex;
        if (Type == EnemyType.Ground)
        {
            var spawnPoints = GameManager.Instance.GroundSpawnPoints;
            spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            for (int i = 0; i < Amount; i++)
            {
                GameManager.Instance.GetGroundEnemy(spawnPoints[spawnPointIndex].position);
                spawnPointIndex++;
                if (spawnPointIndex >= spawnPoints.Length) spawnPointIndex = 0;
            }
        }
        else if (Type == EnemyType.Sky)
        {
            var spawnPoints = GameManager.Instance.SkySpawnPoints;
            spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            for (int i = 0; i < Amount; i++)
            {
                GameManager.Instance.GetSkyEnemy(spawnPoints[spawnPointIndex].position);
                spawnPointIndex++;
                if (spawnPointIndex >= spawnPoints.Length) spawnPointIndex = 0;
            }
        }
        else GameManager.Instance.ResetSpawnTime();
    }
}
public enum EnemyType { None, Ground, Sky}
