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
}
