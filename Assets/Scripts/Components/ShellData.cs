using Unity.Entities;

namespace DefenseGame
{
    public struct ShellData : IComponentData
    {
        public float movementSpeed;
    }

    public class ShellBaker : Baker<Shell>
    {
        public override void Bake(Shell authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShellData
            {
                movementSpeed = authoring.movementSpeed,
            });
        }
    }
}
