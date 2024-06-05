using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEditor;
using UnityEngine;

namespace Wave
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private LevelConfig _level;
        public List<WaveConfig> waves;
        public Transform[] spawnPoints;
        [SerializeField] private GameObject _spawnManagerObject;
        private int _currentWaveIndex;
        private SpawnManager _spawnManager;
        [SerializeField] private GameManager _gameManagerObject;
        private GameManager _gameManager;
        
        private void Start()
        {
            _spawnManager = _spawnManagerObject.GetComponent<SpawnManager>();
            _gameManager = _gameManagerObject.GetComponent<GameManager>();
            _gameManager.OnGameStart.AddListener(StartWave);
            
            waves = _level.Waves;
        }

        private void StartWave()
        {

            StartCoroutine(StartDelay());
            
        }

        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(5);
            var currentWave = waves[_currentWaveIndex];

            foreach (var troop in currentWave.troops)
            {
                foreach (var warriorType in troop.warriorTypes)
                {
                    _spawnManager.InstantiateEnemy(warriorType);
                }
            }
        }
        
        private IEnumerator SpawnWave()
        {
            
            yield break;
            
            // for (var i = 0; i < currentWave.enemyCount; i++)
            // {
            //     SpawnEnemy(currentWave.enemyPrefabs[Random.Range(0, currentWave.enemyPrefabs.Length)]);
            //     yield return new WaitForSeconds(currentWave.spawnInterval);
            // }
            //
            // // Move to the next wave
            // _currentWaveIndex++;
            // if (_currentWaveIndex < waves.Length)
            // {
            //     yield return new WaitForSeconds(5f); // Wait before starting the next wave
            //     StartCoroutine(SpawnWave());
            // }
        }
    }
}