using Unity.Entities;

namespace DefenseGame
{
    public struct PlayerData : IComponentData
    {
        public float movementSpeed;
        public float maxHp;
        public float hp;
        public float experience;
        public int level;
    }
}
