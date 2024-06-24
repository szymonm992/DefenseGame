using Unity.Entities;
using UnityEngine;
using static DefenseGame.Unit;

namespace DefenseGame
{
    public class Enemy : Unit
    {
        public float ExperienceForKill => experienceForKill;

        [SerializeField] private float experienceForKill = 5f;
    }

    public class EnemyBaker : UnitBaker<Enemy>
    {
        public override void Bake(Enemy authoring)
        {
            base.Bake(authoring);

            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new EnemyData
            {
                maxHp = authoring.MaxHp,
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

