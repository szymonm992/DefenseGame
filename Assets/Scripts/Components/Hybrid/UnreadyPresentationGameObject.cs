using Unity.Entities;
using UnityEngine;

namespace DefenseGame
{
    public class UnreadyPresentationGameObject : IComponentData
    {
        public PresentationalGameObject Prefab;
    }

    public class EntityGameObjectSyncTag : ICleanupComponentData
    {
        public Transform Transform;
    }
}
