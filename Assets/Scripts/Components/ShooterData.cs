using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public struct ShooterData : IComponentData
    {
        public Entity shellPrefab;
        public Entity shootingSpotEntity;
        public float shotCooldown;
        public float lastShotTime;
    }

    public class ShooterBaker : Baker<Shooter>
    {
        public override void Bake(Shooter authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var shootingSpotEntity = GetEntity(authoring.shootingSpot, TransformUsageFlags.Dynamic);

            AddComponent(entity, new ShooterData
            {
                shellPrefab = GetEntity(authoring.shellPrefab, TransformUsageFlags.Dynamic),
                shootingSpotEntity = shootingSpotEntity,
                shotCooldown = authoring.shotCooldown,
                lastShotTime = 0f
            });
        }
    }
}
