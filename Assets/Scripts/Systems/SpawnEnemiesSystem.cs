using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.UIElements;

namespace DefenseGame
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(ShootingSystem))]
    public partial struct SpawnEnemiesSystem : ISystem
    {
        public const float ENEMIES_POSITION_Y = 1.2f;

        private Random random;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemySpawnSettings>();
            random = new Random((uint)UnityEngine.Random.Range(1, 100000));
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.TryGetSingletonEntity<GameOverTag>(out _))
            {
                return;
            }

            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (spawnConfig, enemyPrefabsBuffer) in SystemAPI.Query<RefRW<EnemySpawnSettings>, DynamicBuffer<EnemyPrefabEntry>>())
            {
                spawnConfig.ValueRW.timeSinceLastSpawn += deltaTime;

                if (spawnConfig.ValueRW.timeSinceLastSpawn >= spawnConfig.ValueRO.spawnInterval)
                {
                    spawnConfig.ValueRW.timeSinceLastSpawn = 0;

                    float3 spawnPosition = new float3(
                        random.NextFloat(spawnConfig.ValueRO.mapOffset.x, spawnConfig.ValueRO.mapOffset.x + spawnConfig.ValueRO.mapDimensions.x),
                        ENEMIES_POSITION_Y,
                        random.NextFloat(spawnConfig.ValueRO.mapOffset.z, spawnConfig.ValueRO.mapOffset.z + spawnConfig.ValueRO.mapDimensions.z)
                    );
                    float3 lookDirection = math.normalize(new float3(0f, 0f, -1f));
                    quaternion spawnRotation = quaternion.LookRotation(lookDirection, math.up());
                   
                    float totalChance = 0;
                    for (int i = 0; i < enemyPrefabsBuffer.Length; i++)
                    {
                        totalChance += enemyPrefabsBuffer[i].spawnChance;
                    }

                    float randomValue = random.NextFloat(0, totalChance);
                    Entity selectedPrefab = Entity.Null;
                    float cumulativeChance = 0;

                    for (int i = 0; i < enemyPrefabsBuffer.Length; i++)
                    {
                        cumulativeChance += enemyPrefabsBuffer[i].spawnChance / totalChance;
                        if (randomValue < cumulativeChance)
                        {
                            selectedPrefab = enemyPrefabsBuffer[i].prefab;
                            break;
                        }
                    }

                    if (selectedPrefab != Entity.Null)
                    {
                        Entity enemy = ecb.Instantiate(selectedPrefab);
                        float prefabScale = state.EntityManager.GetComponentData<LocalTransform>(selectedPrefab).Scale;
                        ecb.SetComponent(enemy, new LocalTransform
                        {
                            Position = spawnPosition,
                            Rotation = spawnRotation,
                            Scale = prefabScale,
                        });
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("No valid prefab selected for spawning.");
                    }
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
