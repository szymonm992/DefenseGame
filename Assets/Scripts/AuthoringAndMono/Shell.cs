using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class Shell : MonoBehaviour
    {
        public float MovementSpeed => movementSpeed;
        public float Damage => damage;

        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float damage = 1f;
    }

    public class ShellBaker : Baker<Shell>
    {
        public override void Bake(Shell authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new StraightMovementData
            {
                movementSpeed = authoring.MovementSpeed,
            });
            AddComponent(entity, new ShellData
            {
                damage = authoring.Damage,
            });
        }
    }
}
