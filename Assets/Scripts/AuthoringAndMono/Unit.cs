using UnityEngine;

namespace DefenseGame
{
    public abstract class Unit : MonoBehaviour
    {
        public float MaxHp => maxHp;
        public float MovementSpeed => movementSpeed;

        [SerializeField] private float maxHp = 10f;
        [SerializeField] private float movementSpeed = 2f;
    }
}
