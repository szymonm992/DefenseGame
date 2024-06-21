using Unity.Entities;

namespace DefenseGame
{
    public struct CharacterData : IComponentData
    {
        public float movementSpeed;
        public float hp;
        public float experience;
        public int level;
    }
}
