using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace DefenseGame
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct EnemyCollisionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var shellTagHandle = state.GetComponentLookup<ShellData>(true);
            var enemyTagHandle = state.GetComponentLookup<EnemyData>(true);

            var collisionJob = new CollisionEventJob
            {
                shellTagLookup = shellTagHandle,
                enemyTagLookup = enemyTagHandle,
                ecb = ecb.AsParallelWriter()
            };

            state.Dependency = collisionJob.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        struct CollisionEventJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentLookup<ShellData> shellTagLookup;
            [ReadOnly] public ComponentLookup<EnemyData> enemyTagLookup;
            public EntityCommandBuffer.ParallelWriter ecb;

            public void Execute(CollisionEvent collisionEvent)
            {
                var firstEntity = collisionEvent.EntityA;
                var secondEntity = collisionEvent.EntityB;

                bool isFirstEntityShell = shellTagLookup.HasComponent(firstEntity);
                bool isSecondEntityShell = shellTagLookup.HasComponent(secondEntity);
                bool isFirstEntityEnemy = enemyTagLookup.HasComponent(firstEntity);
                bool isSecondEntityEnemy = enemyTagLookup.HasComponent(secondEntity);

                if ((isFirstEntityShell && isSecondEntityEnemy) || (isSecondEntityShell && isFirstEntityEnemy))
                {
                    ecb.DestroyEntity(0, firstEntity);
                    ecb.DestroyEntity(0, secondEntity);
                }
            }
        }
    }
}
