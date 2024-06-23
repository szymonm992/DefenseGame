using Unity.Entities;

[InternalBufferCapacity(10)]
public struct EnemyPrefabEntry : IBufferElementData
{
    public Entity prefab;
    public float spawnChance;
}
