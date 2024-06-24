using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public abstract class Unit : MonoBehaviour
    {
        public PresentationalGameObject PresentationModel => presentationModel;
        public float MaxHp => maxHp;
        public float MovementSpeed => movementSpeed;

        [SerializeField] private float maxHp = 10f;
        [SerializeField] private float movementSpeed = 2f;
        [SerializeField] private PresentationalGameObject presentationModel;

        public abstract class UnitBaker<T> : Baker<T> where T : Unit
        {
            public override void Bake(T authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponentObject(entity, new UnreadyPresentationGameObject
                {
                    Prefab = authoring.PresentationModel
                });
            }
        }
    }
}
