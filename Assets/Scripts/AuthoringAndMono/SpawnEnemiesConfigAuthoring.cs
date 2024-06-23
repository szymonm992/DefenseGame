using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class EnemySpawnSettingsAuthoring : MonoBehaviour
    {
        public List<EnemySpawnEntry> enemySpawnEntries;
        public Vector3 mapDimensions;
        public Vector3 mapOffset;
        public float spawnInterval = 1f;

        public class Baker : Baker<EnemySpawnSettingsAuthoring>
        {
            public override void Bake(EnemySpawnSettingsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                DynamicBuffer<EnemyPrefabEntry> enemyPrefabsBuffer = AddBuffer<EnemyPrefabEntry>(entity);
                for (int i = 0; i < authoring.enemySpawnEntries.Count; i++)
                {
                    enemyPrefabsBuffer.Add(new EnemyPrefabEntry
                    { 
                        prefab = GetEntity(authoring.enemySpawnEntries[i].enemyPrefab, TransformUsageFlags.Dynamic),
                        spawnChance = authoring.enemySpawnEntries[i].spawnChance
                    });
                }

                AddComponent(entity, new EnemySpawnSettings
                {
                    spawnInterval = authoring.spawnInterval,
                    mapDimensions = authoring.mapDimensions,
                    mapOffset = authoring.mapOffset,
                    timeSinceLastSpawn = 0,
                });
            }
        }
    }
}
