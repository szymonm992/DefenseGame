using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class Enemy : MonoBehaviour
    {
        public float hp;
        public float movementSpeed = 2f;
    }

    public class EnemyBaker : Baker<Enemy>
    {
        public override void Bake(Enemy authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyData
            {
                hp = authoring.hp   
            });
            AddComponent(entity, new StraightMovementData
            {
                movementSpeed = authoring.movementSpeed,
            });
        }
    }
}
