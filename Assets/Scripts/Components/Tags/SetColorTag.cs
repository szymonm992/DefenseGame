using Unity.Entities;
using Unity.Mathematics;

namespace DefenseGame
{
    public struct SetColorTag : IComponentData
    {
        public float4 colorRGBA;
    }
}
