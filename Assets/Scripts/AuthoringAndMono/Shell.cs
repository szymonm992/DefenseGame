using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class Shell : MonoBehaviour
    {
        public float MovementSpeed => movementSpeed;

        [SerializeField] private float movementSpeed = 10f;
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
            AddComponent(entity, new ShellTag());
        }
    }
}
