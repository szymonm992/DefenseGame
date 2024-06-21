using UnityEngine;

namespace DefenseGame
{
    public class Shooter : MonoBehaviour
    {
        public float shotCooldown = 0.2f;
        public GameObject shellPrefab;
        public Transform shootingSpot;
    }
}
