using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace DefenseGame
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    [UpdateAfter(typeof(ShootingSystem))] // Adjust according to your actual system dependencies
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

            var time = SystemAPI.Time.ElapsedTime;
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (spawnConfig, enemyPrefabsBuffer, enemyColorsBuffer) in SystemAPI.Query<RefRW<EnemySpawnSettings>, DynamicBuffer<EnemyPrefabEntry>, DynamicBuffer<EnemyColorEntry>>())
            {
                if ((time - spawnConfig.ValueRW.lastSpawnTime) >= spawnConfig.ValueRO.spawnInterval)
                {
                    spawnConfig.ValueRW.lastSpawnTime = (float)time;

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

                        int randomColorIndex = random.NextInt(0, enemyColorsBuffer.Length);
                        float4 randomColor = enemyColorsBuffer[randomColorIndex].color;
                        //UnityEngine.Debug.Log($"Selected color: r: {randomColor.x} g: {randomColor.y} b: {randomColor.z} a: {randomColor.w}");

                        ecb.AddComponent(enemy, new SetColorTag
                        {
                            colorRGBA = randomColor,
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
