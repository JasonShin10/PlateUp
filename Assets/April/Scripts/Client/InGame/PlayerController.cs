using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.UI;

namespace April
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        public InteractionBase currentInteractionObject;
        public GameObject item;

        public float interactionOffsetHeight = 0.8f;
        public LayerMask interactionObjectLayerMask;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            // Ray�� �������� �÷��̾��� �� �Ʒ��� �ƴ϶�, �ణ ������ �����ϰ� �ϴ°�
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out var hitInfo, 1f, interactionObjectLayerMask, QueryTriggerInteraction.Collide))
            {
                if (hitInfo.transform.TryGetComponent<InteractionBase>(out var interaction))
                {
                    if (currentInteractionObject != null && currentInteractionObject != interaction)
                    {
                        currentInteractionObject = interaction;
                        interaction.Interact(this);
                        Debug.Log("1");
                    }
                    else if (currentInteractionObject == null)
                    {
                        currentInteractionObject = interaction;
                        interaction.Interact(this);
                        Debug.Log("2");
                    }
                    else
                    {
                        // Same Interaction Object -> Do Nothing                        
                    }
                }
            }
            else
            {
                UIManager.Hide<InteractionUI>(UIList.InteractionUI);
                currentInteractionObject = null;
            }
        }

        public void ExitInteractionObject()
        {
            if (currentInteractionObject != null)
            {
                currentInteractionObject.Exit();
            }
        }
    }
}

