using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class Shooter : MonoBehaviour
    {
        public float shotCooldown = 0.2f;
        public float shotDamage = 1f;
        public GameObject shellPrefab;
        public Transform shootingSpot;
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
                shotDamage = authoring.shotDamage,
                lastShotTime = 0f
            });
        }
    }
}
