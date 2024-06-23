using Unity.Entities;

namespace DefenseGame
{
    public struct EnemyData : IComponentData
    {
        public float maxHp;
        public float hp;
        public float experienceForKill;
    }
}
