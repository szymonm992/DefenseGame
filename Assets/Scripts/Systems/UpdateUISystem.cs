using Unity.Burst;
using Unity.Entities;

namespace DefenseGame
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct UpdateUISystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerData>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            if (Menu.Instance == null)
            {
                return;
            }

            Menu.Instance.Initialize();
        }
    }
}
