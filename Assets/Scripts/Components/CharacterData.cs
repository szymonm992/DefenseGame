using Unity.Entities;

namespace DefenseGame
{
    public struct CharacterData : IComponentData
    {
        public float movementSpeed;
        public float hp;
    }

    public class CharacterBaker : Baker<Character>
    {
        public override void Bake(Character authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterData
            {
                movementSpeed = authoring.movementSpeed,
                hp = authoring.hp,
            });
            AddComponent(entity, new InputsData());
        }
    }
}
