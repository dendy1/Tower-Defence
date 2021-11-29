using System;
using System.Collections;
using System.Collections.Generic;
using Core.Utilities.ObjectPool;
using TowerDefense.Enemies;
using TowerDefense.Levels;
using UnityEngine;

namespace TowerDefense.Spawner
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private List<WaveInfo> waves;
        [SerializeField] private Transform waypointsHolder;
    
        public event Action WaveIsOver;
        public event Action<Enemy> CreepSpawned;
    
        private bool _waveIsSpawning;
        private Vector3[] _pathWaypoints;
        private float _pathDistance;

        private void Awake()
        {
            _pathWaypoints = waypointsHolder.GetChildrenPosition();
            _pathDistance = waypointsHolder.GetWaypointsDistance();
        }

        private void Start()
        {
            GameManager.Instance.WavesCount = waves.Count;
        }
    
        public void SpawnWave(int wave)
        {
            foreach (var creepWaveInfo in waves[wave].enemies)
            {
                GameManager.Instance.EnemiesRemaining += creepWaveInfo.Count;
            }
            StartCoroutine(SpawnWaveCoroutine(wave));
        }
        
        private IEnumerator SpawnWaveCoroutine(int wave)
        {
            var waveInfo = waves[wave];
    
            foreach (var creepWaveInfo in waveInfo.enemies)
            {
                StartCoroutine(SpawnCreepWave(creepWaveInfo));
                yield return new WaitUntil(() => !_waveIsSpawning);
            }
            
            WaveIsOver?.Invoke();
        }
        
        private IEnumerator SpawnCreepWave(EnemyWaveInfo enemyWaveInfo)
        {
            _waveIsSpawning = true;
            for (var i = 0; i < enemyWaveInfo.Count; i++)
            {
                SpawnCreep(enemyWaveInfo.enemy);
                yield return new WaitForSeconds(enemyWaveInfo.SpawnDelay);
            }
            _waveIsSpawning = false;
        }
    
        private void SpawnCreep(Enemy enemy)
        {
            var spawnedEnemy = Poolable.TryGetPoolable<Enemy>(enemy.gameObject);
            spawnedEnemy.transform.position = _pathWaypoints[0];
            spawnedEnemy.transform.forward = _pathWaypoints[1] - _pathWaypoints[0];
            spawnedEnemy.InitializeEnemy(_pathWaypoints);
            CreepSpawned?.Invoke(spawnedEnemy);
        }
    }
}