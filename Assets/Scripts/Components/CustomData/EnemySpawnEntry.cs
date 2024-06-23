using UnityEngine;

namespace DefenseGame
{
    [System.Serializable]
    public class EnemySpawnEntry
    {
        public GameObject enemyPrefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }
}
