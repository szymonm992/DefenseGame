using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;

namespace DefenseGame
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial struct ShootingSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            {
                var time = SystemAPI.Time.ElapsedTime;
                var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

                foreach (var (inputs, shooter) in SystemAPI.Query<RefRO<InputsData>, RefRW<ShooterData>>())
                {
                    if (inputs.ValueRO.isShooting && (time - shooter.ValueRW.lastShotTime) >= shooter.ValueRW.shotCooldown)
                    {
                        shooter.ValueRW.lastShotTime = (float)time;
                        var shellEntity = ecb.Instantiate(shooter.ValueRO.shellPrefab);

                        // Get the LocalToWorld transform to obtain the world space position
                        var shootingSpotTransform = state.EntityManager.GetComponentData<LocalToWorld>(shooter.ValueRW.shootingSpotEntity);

                        ecb.SetComponent(shellEntity, new LocalTransform
                        {
                            Position = shootingSpotTransform.Position,
                            Rotation = quaternion.identity,
                            Scale = 1f
                        });
                    }
                }

                ecb.Playback(state.EntityManager);
                ecb.Dispose();
            }
        }
    }
}
