using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace DefenseGame
{
    public partial struct  PlayerMovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (SystemAPI.TryGetSingletonEntity<GameOverTag>(out _))
            {
                return;
            }

            foreach (var (data, inputs, transform) in SystemAPI.Query<RefRO<PlayerData>, RefRO<InputsData>, RefRW<LocalTransform>>())
            { 
                float3 offsetPosition = transform.ValueRO.Position;
                offsetPosition.x += inputs.ValueRO.movement.x * data.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
                transform.ValueRW.Position = offsetPosition;
            }
        }
    }
}
