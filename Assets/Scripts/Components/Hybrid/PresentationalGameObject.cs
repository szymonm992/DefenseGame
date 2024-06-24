using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefenseGame
{
    public class PresentationalGameObject : MonoBehaviour
    {
        [SerializeField] private Renderer assignedRenderer;

        public void SyncTransform(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public void SetColor(Color color)
        {
            if (assignedRenderer != null)
            {
                assignedRenderer.material.color = color;
            }
            else
            {
                Debug.LogWarning($"GameObject {gameObject.name} does not have a Renderer component.");
            }
        }
    }
}
