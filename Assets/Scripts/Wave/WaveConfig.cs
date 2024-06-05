using System.Collections.Generic;
using UnityEngine;

namespace Wave
{
    [CreateAssetMenu(fileName = "NewWave", menuName = "Game/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        public int waveId;
        public string waveName;
        public List<TroopConfig> troops;
        public int enemyCount;
        public float spawnInterval;
    }
}