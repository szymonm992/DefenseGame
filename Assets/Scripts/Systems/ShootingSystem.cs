using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

namespace DefenseGame
{
    public partial struct ShootingSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var time = SystemAPI.Time.ElapsedTime;

            foreach (var (inputs, shooter) in SystemAPI.Query<RefRO<InputsData>, RefRW<ShooterData>>())
            {
                if (inputs.ValueRO.isShooting && (time - shooter.ValueRW.lastShotTime) >= shooter.ValueRW.shotCooldown)
                {
                    shooter.ValueRW.lastShotTime = (float)time;
                    var shellEntity = state.EntityManager.Instantiate(shooter.ValueRO.shellPrefab);

                    // Get the LocalToWorld transform to obtain the world space position
                    var shootingSpotTransform = state.EntityManager.GetComponentData<LocalToWorld>(shooter.ValueRO.shootingSpotEntity);

                    state.EntityManager.SetComponentData(shellEntity, new LocalTransform
                    {
                        Position = shootingSpotTransform.Position,
                        Rotation = quaternion.identity,
                        Scale = 1f
                    });
                }
            }
        }
    }
}
