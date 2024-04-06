using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Transform _groundSpawnPoint;
    public Transform _skySpawnPoint;
    [SerializeField] int[] _phase;
    [SerializeField] GroundEnemy _groundEnemyPrefab;
    [SerializeField] SkyEnemy _skyEnemyPrefab;
    Queue<GroundEnemy> groundEnemies = new();
    Queue<SkyEnemy> skyEnemyes = new();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
