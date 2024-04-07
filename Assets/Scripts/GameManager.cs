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

    public ThroneHealth Throne;

    [HideInInspector] public Queue<GroundEnemy> GroundEnemies = new();
    [HideInInspector] public Queue<SkyEnemy> SkyEnemies = new();
    [HideInInspector] public Queue<BulletController> Bullets = new();

    public event Action ON_GAME_OVER;
    public Transform[] GroundPath => _groundPath;
    public Transform Canvas => _canvas;
    public Transform BulletPool => _bulletPool;

    float _timeToSpawn;
    int _phaseCount;
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        StartSpawnEnemy(_timeToSpawn);
        _timeToSpawn += Time.deltaTime;
    }

    void StartSpawnEnemy(float time)
    {
        if (time >= _phases[_phaseCount].TimeToSpawn)
        {
            SpawnEnemy(_phases[_phaseCount]);
            _timeToSpawn = 0;
            _phaseCount++;            
            if (_phaseCount >= _phases.Length) _phaseCount = 0; 
        }
    }

    void SpawnEnemy(SpawningPattern phase)
    {
        if (phase.Type == EnemyType.Ground)
        {
            for (int i = 0; i < phase.Amount; i++)
            {
                GetGroundEnemy(_groundSpawnPoints[i].position);
            }
        }
        else if (phase.Type == EnemyType.Sky)
        {
            var startPointIndex = UnityEngine.Random.Range(0, _skySpawnPoints.Length);
            for (int i = 0; i < phase.Amount; i++)
            {
                GetSkyEnemy(_skySpawnPoints[startPointIndex].position);
                startPointIndex++;
                if (startPointIndex >= _skySpawnPoints.Length) startPointIndex = 0;
            }
        }
    }
    void GetGroundEnemy(Vector2 position)
    {
        GroundEnemy enemy;
        if (GroundEnemies.Count > 0) enemy = GroundEnemies.Dequeue();
        else
        {
            enemy = Instantiate(_groundEnemyPrefab, _enemyPool);
            enemy.Init();
        }
        enemy.Respawn(position);
    }
    void GetSkyEnemy(Vector2 position)
    {
        SkyEnemy enemy;
        if (SkyEnemies.Count > 0) enemy = SkyEnemies.Dequeue();
        else
        {
            enemy = Instantiate(_skyEnemyPrefab, _enemyPool);
            enemy.Init();
        }
        enemy.Respawn(position);
    }

    public void ResetSpawnTime() => _timeToSpawn = 0;

    public void FireGameOver() => ON_GAME_OVER?.Invoke();
}

[Serializable]
public class SpawningPattern
{
    public float TimeToSpawn;
    public EnemyType Type;
    public int Amount;
}
public enum EnemyType { None, Ground, Sky}
