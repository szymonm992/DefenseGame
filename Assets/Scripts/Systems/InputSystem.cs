using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public partial class InputsSystem : SystemBase
    {
        private Inputs inputs = null;

        protected override void OnCreate()
        {
            inputs = new Inputs();
            inputs.Enable();
        }

        protected override void OnUpdate()
        {
            if (inputs == null)
            {
                return;
            }

            foreach (RefRW<InputsData> inputData in SystemAPI.Query<RefRW<InputsData>>())
            {
                inputData.ValueRW.movement = inputs.Player.Move.ReadValue<Vector2>();
            }
        }
    }
}
