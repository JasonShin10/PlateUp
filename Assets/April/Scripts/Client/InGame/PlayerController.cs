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
            // Ray�� �������� �÷��̾��� �� �Ʒ��� �ƴ϶�, �ణ ������ �����ϰ� �ϴ°�
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

