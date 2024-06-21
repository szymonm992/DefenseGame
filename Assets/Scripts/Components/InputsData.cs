using Unity.Entities;
using Unity.Mathematics;

namespace DefenseGame
{
    public struct InputsData : IComponentData
    {
        public float2 movement;
        public bool isShooting;
        public bool pressedAreaAttack;
    }
}
