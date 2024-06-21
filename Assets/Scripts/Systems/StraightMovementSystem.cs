using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace DefenseGame
{
    public partial struct StraightMovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (data, transform) in SystemAPI.Query<RefRO<StraightMovementData>, RefRW<LocalTransform>>())
            {
                float3 forward = math.forward(transform.ValueRO.Rotation);
                float3 offsetPosition = transform.ValueRO.Position;
                offsetPosition += forward * data.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
                transform.ValueRW.Position = offsetPosition;
            }
        }
    }
}
