using Unity.Entities;
using Unity.Mathematics;

public struct EnemyPrefabEntry : IBufferElementData
{
    public Entity prefab;
    public float spawnChance;
}
