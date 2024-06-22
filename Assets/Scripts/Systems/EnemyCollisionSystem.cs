using System;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace DefenseGame
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct ShellCollisionSystem : ISystem, ISystemStartStop
    {
        private Entity playerEntity;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<PlayerData>();
        }

        [BurstCompile]
        public void OnStartRunning(ref SystemState state)
        {
            playerEntity = SystemAPI.GetSingletonEntity<PlayerData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            var shellTagHandle = SystemAPI.GetComponentLookup<ShellData>(true);
            var enemyTagHandle = SystemAPI.GetComponentLookup<EnemyData>(false);
            var playerTagHandle = SystemAPI.GetComponentLookup<PlayerData>(false);
            
            var collisionJob = new ShellCollisionEventJob
            {
                shellTagLookup = shellTagHandle,
                enemyTagLookup = enemyTagHandle,
                playerTagLookup = playerTagHandle,
                playerEntity = playerEntity,
                ecb = ecb.AsParallelWriter()
            };

            state.Dependency = collisionJob.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        public void OnStopRunning(ref SystemState state)
        {
        }

        [BurstCompile]
        struct ShellCollisionEventJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentLookup<ShellData> shellTagLookup;
            [ReadOnly] public Entity playerEntity;
            public ComponentLookup<EnemyData> enemyTagLookup;
            public ComponentLookup<PlayerData> playerTagLookup;
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
                    var shellEntity = isFirstEntityShell ? firstEntity : secondEntity;
                    var enemyEntity = isFirstEntityEnemy ? firstEntity : secondEntity;

                    var shellData = shellTagLookup[shellEntity];
                    var enemyData = enemyTagLookup[enemyEntity];
                    var playerData = playerTagLookup[playerEntity];

                    if ((enemyData.hp - shellData.damage) > 0)
                    {
                        enemyData.hp -= shellData.damage;
                        enemyTagLookup[enemyEntity] = enemyData;
                    }
                    else
                    {
                        playerData.experience += enemyData.experienceForKill;
                        playerTagLookup[playerEntity] = playerData;
                        ecb.DestroyEntity(0, enemyEntity);
                    }

                    ecb.DestroyEntity(0, shellEntity);
                }
            }
        }
    }
}
