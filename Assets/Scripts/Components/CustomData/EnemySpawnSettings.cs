using Unity.Entities;
using Unity.Mathematics;

public struct EnemySpawnSettings : IComponentData
{
    public float timeSinceLastSpawn;
    public float3 mapDimensions;
    public float3 mapOffset;
    public float spawnInterval;
}