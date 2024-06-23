using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class Player : Unit
    {
        public float Experience => experience;
        public int Level => level;

        [SerializeField] private float experience = 0f; 
        [SerializeField] private int level = 1;
    }

    public class PlayerBaker : Baker<Player>
    {
        public override void Bake(Player authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerData
            {
                movementSpeed = authoring.MovementSpeed,
                maxHp = authoring.MaxHp,
                hp = authoring.MaxHp,
                experience = authoring.Experience,
                level = authoring.Level,
            });
            AddComponent(entity, new InputsData());
        }
    }
}
