using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.UI;

namespace April
{
    public class PlayerController : MonoBehaviour
    {
        public InteractionBase currentInteractionObject;
        public GameObject item;

        public float interactionOffsetHeight = 0.8f;
        public LayerMask interactionObjectLayerMask;

        private void Update()
        {
            currentInteractionObject = null;
            // Ray의 시작점을 플레이어의 발 아래가 아니라, 약간 위에서 시작하게 하는것
            Ray ray = new Ray(transform.position - (Vector3.up * interactionOffsetHeight), Vector3.down);
            if (Physics.Raycast(ray, out var hitInfo, 1f, interactionObjectLayerMask, QueryTriggerInteraction.Collide) && item == null)
            {
                if (hitInfo.transform.TryGetComponent<InteractionBase>(out var interaction))
                {
                   currentInteractionObject = interaction;
                    item = currentInteractionObject.Interact(this.gameObject.transform);
                }
            }
        }
    }
}

