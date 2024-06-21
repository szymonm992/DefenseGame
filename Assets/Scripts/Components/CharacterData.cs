using Unity.Entities;

namespace DefenseGame
{
    public struct CharacterData : IComponentData
    {
        public float movementSpeed;
    }

    public class CharacterBaker : Baker<Character>
    {
        public override void Bake(Character authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterData
            {
                movementSpeed = authoring.movementSpeed,
            });
            AddComponent(entity, new InputsData());
        }
    }
}
