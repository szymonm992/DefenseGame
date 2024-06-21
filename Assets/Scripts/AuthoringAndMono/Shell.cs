using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class Shell : MonoBehaviour
    {
        public float movementSpeed = 10f;
        public float damage = 1f;
    }

    public class ShellBaker : Baker<Shell>
    {
        public override void Bake(Shell authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new StraightMovementData
            {
                movementSpeed = authoring.movementSpeed,
            });
            AddComponent(entity, new ShellData
            {
                damage = authoring.damage,
            });
        }
    }
}
