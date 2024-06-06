using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Wave
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private LevelConfig _level;
        [SerializeField] private GameObject _spawnManagerObject;
        [SerializeField] private GameManager _gameManagerObject;
        [SerializeField] private GameObject _waveInfoObject;
        [SerializeField] private TMP_Text _waveInfo;
        [SerializeField] private float waveDelay;
        private GameManager _gameManager;
        private SpawnManager _spawnManager;
        private List<WaveConfig> waves;

        private void Start()
        {
            _spawnManager = _spawnManagerObject.GetComponent<SpawnManager>();
            _gameManager = _gameManagerObject.GetComponent<GameManager>();
            _gameManager.OnGameStart.AddListener(StartWave);
            _waveInfoObject.SetActive(false);

            waves = _level.Waves;
        }

        private void StartWave()
        {
            StartCoroutine(StartDelay());
        }

        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(5);
            StartCoroutine(InstantiateWave());
        }

        private IEnumerator ShowWaveInfo(WaveConfig currenWave)
        {
            _waveInfoObject.SetActive(true);
            _waveInfo.text = currenWave.waveName;
            yield return new WaitForSeconds(3);
            _waveInfoObject.SetActive(false);
        }

        private IEnumerator InstantiateWave()
        {
            foreach (var currentWave in waves)
            {
                if (_gameManager.GameOver)
                    yield break;

                StartCoroutine(ShowWaveInfo(currentWave));
                SpawnWave(currentWave);

                yield return new WaitForSeconds(waveDelay);
            }
        }

        private void SpawnWave(WaveConfig currentWave)
        {
            foreach (var warriorType in currentWave.warriorTypes) _spawnManager.InstantiateEnemy(warriorType);
        }
    }
}