using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace DefenseGame
{
    public partial struct ShellMovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (data, transform) in SystemAPI.Query<RefRO<ShellData>, RefRW<LocalTransform>>())
            {
                float3 offsetPosition = transform.ValueRO.Position;
                offsetPosition.z += data.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
                transform.ValueRW.Position = offsetPosition;
            }
        }
    }
}
