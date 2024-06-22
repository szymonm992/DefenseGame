using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class Wall : MonoBehaviour
    {
    }

    public class WallBaker : Baker<Wall>
    {
        public override void Bake(Wall authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new WallTag());
        }
    }
}
