using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace DefenseGame
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct GameplayCollisionSystem : ISystem, ISystemStartStop
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
            var shellTagHandle = SystemAPI.GetComponentLookup<ShellTag>(true);
            var wallTagHandle = SystemAPI.GetComponentLookup<WallTag>(true);
            var enemyTagHandle = SystemAPI.GetComponentLookup<EnemyData>(false);
            var playerTagHandle = SystemAPI.GetComponentLookup<PlayerData>(false);
            
            var collisionJob = new GameplayCollisionEventJob
            {
                shellTagLookup = shellTagHandle,
                enemyTagLookup = enemyTagHandle,
                playerTagLookup = playerTagHandle,
                wallTagLookup = wallTagHandle,
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
        struct GameplayCollisionEventJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentLookup<ShellTag> shellTagLookup;
            [ReadOnly] public Entity playerEntity;
            [ReadOnly] public ComponentLookup<WallTag> wallTagLookup;
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
                bool isFirstEntityWall = wallTagLookup.HasComponent(firstEntity);
                bool isSecondEntityWall = wallTagLookup.HasComponent(secondEntity);

                var playerData = playerTagLookup[playerEntity];
                var enemyEntity = isFirstEntityEnemy ? firstEntity : secondEntity;

                if ((isFirstEntityShell && isSecondEntityEnemy) || (isSecondEntityShell && isFirstEntityEnemy))
                {
                    var shellEntity = isFirstEntityShell ? firstEntity : secondEntity;
                    var shellData = shellTagLookup[shellEntity];
                    var enemyData = enemyTagLookup[enemyEntity];

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
                else if ((isFirstEntityWall && isSecondEntityEnemy) || (isSecondEntityWall && isFirstEntityEnemy))
                {
                    if ((playerData.hp - 1) > 0)
                    {
                        playerData.hp -= 1;
                        ecb.DestroyEntity(0, enemyEntity);
                    }
                    else
                    {
                        playerData.hp = 0;
                        ecb.AddComponent(0, playerEntity, new GameOverTag
                        {
                            experienceGained = playerData.experience,
                            levelReached = playerData.level,
                        });
                    }

                    playerTagLookup[playerEntity] = playerData;
                }
            }
        }
    }
}
