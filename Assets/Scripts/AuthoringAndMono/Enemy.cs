using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class Enemy : Unit
    {
        public float ExperienceForKill => experienceForKill;

        [SerializeField] private float experienceForKill = 5f;
    }

    public class EnemyBaker : Baker<Enemy>
    {
        public override void Bake(Enemy authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyData
            {
                hp = authoring.MaxHp,
                experienceForKill = authoring.ExperienceForKill,

            });
            AddComponent(entity, new StraightMovementData
            {
                movementSpeed = authoring.MovementSpeed,
            });
        }
    }
}
