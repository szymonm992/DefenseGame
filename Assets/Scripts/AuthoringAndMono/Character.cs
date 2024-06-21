using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class Character : MonoBehaviour
    {
        public float movementSpeed = 2f;
        public float hp = 10f;
        public float experience = 0;
        public int level = 1;
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
                experience = authoring.experience,
                level = authoring.level,
            });
            AddComponent(entity, new InputsData());
        }
    }
}
