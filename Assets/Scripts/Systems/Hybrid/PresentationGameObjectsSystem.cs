using DefenseGame;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct PresentationGameObjectsSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var colorTagHandle = SystemAPI.GetComponentLookup<SetColorTag>(true);

        foreach (var (pgo, transform, entity) in SystemAPI.Query<UnreadyPresentationGameObject, LocalToWorld>().WithEntityAccess())
        {
            PresentationalGameObject go = GameObject.Instantiate(pgo.Prefab, new Vector3(transform.Position.x, transform.Position.y, transform.Position.z), Quaternion.identity);

            if (colorTagHandle.HasComponent(entity))
            {
                go.SetColor(new Color(colorTagHandle[entity].colorRGBA.x, colorTagHandle[entity].colorRGBA.y, colorTagHandle[entity].colorRGBA.z, colorTagHandle[entity].colorRGBA.w));
                ecbBOS.RemoveComponent<SetColorTag>(entity);
            }

            ecbBOS.AddComponent(entity, new EntityGameObjectSyncTag() { Transform = go.transform });
            ecbBOS.RemoveComponent<UnreadyPresentationGameObject>(entity);
        }

        foreach (var (goTransform, tranform) in SystemAPI.Query<EntityGameObjectSyncTag, LocalToWorld>())
        {
            goTransform.Transform.position = tranform.Position;
            goTransform.Transform.rotation = tranform.Rotation;
        }

        var ecbEOS = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (goTransform, entity) in SystemAPI.Query<EntityGameObjectSyncTag>().WithNone<LocalToWorld>().WithEntityAccess())
        {
            if (goTransform.Transform != null)
            {
                GameObject.Destroy(goTransform.Transform.gameObject);
            }

            ecbEOS.RemoveComponent<UnreadyPresentationGameObject>(entity);
        }
    }
}