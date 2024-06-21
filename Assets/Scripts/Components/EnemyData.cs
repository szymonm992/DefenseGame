using Unity.Entities;

namespace DefenseGame
{
    public struct EnemyData : IComponentData
    {
        public float hp;
        public float movementSpeed;
    }
}
