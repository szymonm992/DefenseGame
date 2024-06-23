using Unity.Entities;

public struct EnemyPrefabEntry : IBufferElementData
{
    public Entity prefab;
    public float spawnChance;
}
