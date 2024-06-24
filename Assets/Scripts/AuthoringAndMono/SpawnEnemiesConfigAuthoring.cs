using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DefenseGame
{
    public class EnemySpawnSettingsAuthoring : MonoBehaviour
    {
        public List<EnemySpawnEntry> enemySpawnEntries;
        public List<Color> enemyColors = new List<Color>
        {
            Color.black,
            Color.blue,
            Color.cyan,
            Color.gray,
            Color.green,
            Color.magenta,
            Color.red,
            Color.white,
            Color.yellow
        };

        public Vector3 mapDimensions;
        public Vector3 mapOffset;
        public float spawnInterval = 1f;

        public class Baker : Baker<EnemySpawnSettingsAuthoring>
        {
            public override void Bake(EnemySpawnSettingsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                DynamicBuffer<EnemyPrefabEntry> enemyPrefabsBuffer = AddBuffer<EnemyPrefabEntry>(entity);
                DynamicBuffer<EnemyColorEntry> enemyColorsBuffer = AddBuffer<EnemyColorEntry>(entity);

                // Add enemy prefab entries
                for (int i = 0; i < authoring.enemySpawnEntries.Count; i++)
                {
                    enemyPrefabsBuffer.Add(new EnemyPrefabEntry
                    {
                        prefab = GetEntity(authoring.enemySpawnEntries[i].enemyPrefab, TransformUsageFlags.Dynamic),
                        spawnChance = authoring.enemySpawnEntries[i].spawnChance
                    });
                }

                // Add enemy color entries
                for (int i = 0; i < authoring.enemyColors.Count; i++)
                {
                    Color color = authoring.enemyColors[i];
                    enemyColorsBuffer.Add(new EnemyColorEntry
                    {
                        color = new float4(color.r, color.g, color.b, color.a)
                    });
                }

                // Add EnemySpawnSettings component
                AddComponent(entity, new EnemySpawnSettings
                {
                    spawnInterval = authoring.spawnInterval,
                    mapDimensions = authoring.mapDimensions,
                    mapOffset = authoring.mapOffset,
                    lastSpawnTime = 0,
                });
            }
        }
    }
}
