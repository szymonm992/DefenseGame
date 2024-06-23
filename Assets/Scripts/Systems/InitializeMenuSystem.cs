using Unity.Entities;

namespace DefenseGame
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitializeMenuSystem : ISystem, ISystemStartStop
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerData>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            if (Menu.Instance == null)
            {
                return;
            }

            Menu.Instance.Initialize();
        }

        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}
