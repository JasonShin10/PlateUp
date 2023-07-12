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
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out var hitInfo, 1f, interactionObjectLayerMask, QueryTriggerInteraction.Collide))
            {
                if (hitInfo.transform.TryGetComponent<FoodContainer>(out var interactionFoodContainer) && item == null)
                {
                    currentInteractionObject = interactionFoodContainer;


                    item = currentInteractionObject.Interact(this.gameObject.transform);


                }
                if (hitInfo.transform.TryGetComponent<Stove>(out var interactionStove) && item != null)
                {
                    currentInteractionObject = interactionStove;
                    currentInteractionObject.foodPrefab = item;
                    Destroy(item);
                    item = currentInteractionObject.Interact(hitInfo.transform);
                    item = null;
                }
            }
        }
    }
}

