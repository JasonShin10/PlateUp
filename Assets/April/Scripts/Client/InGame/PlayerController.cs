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

        public float interactionOffsetHeight = 0.8f;
        public LayerMask interactionObjectLayerMask;

        private void Update()
        {
            currentInteractionObject = null;
            Ray ray = new Ray(transform.position - (Vector3.up * interactionOffsetHeight), Vector3.down);
            if (Physics.Raycast(ray, out var hitInfo, 1f, interactionObjectLayerMask, QueryTriggerInteraction.Collide))
            {
                if (hitInfo.transform.TryGetComponent<InteractionBase>(out var interaction))
                {
                    currentInteractionObject = interaction;
                }
            }
        }
    }
}

