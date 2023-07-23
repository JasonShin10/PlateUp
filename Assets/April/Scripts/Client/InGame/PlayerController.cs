using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.UI;
using TMPro;
using System.Xml.Serialization;

namespace April
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        public InteractionBase currentInteractionObject;
        public GameObject item;

        public float interactionOffsetHeight = 0.8f;
        public LayerMask interactionObjectLayerMask;

        public string playerName;
        public TMPro.TextMeshProUGUI playerNameText;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void OnEnable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Interact.performed += DoInteraction;
            playerNameText.text = playerName;
        }

        private void OnDisable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Interact.performed -= DoInteraction;
        }

        private void DoInteraction(InputAction.CallbackContext context)
        {
            currentInteractionObject?.Interact(this);
        }

        private void Update()
        {
            // Ray�� �������� �÷��̾��� �� �Ʒ��� �ƴ϶�, �ణ ������ �����ϰ� �ϴ°�
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out var hitInfo, 1f, interactionObjectLayerMask, QueryTriggerInteraction.Collide))
            {
                if (hitInfo.transform.TryGetComponent<InteractionBase>(out var interaction))
                {
                    // stove�� �ٶ󺸸� currentInteractionObject�� stove. 
                    // �ٸ��� �ٶ󺼶� �ٸ��� ���ŵǰԲ� �ϴ°ǰ�?
                    if (currentInteractionObject != null && currentInteractionObject != interaction)
                    {
                        currentInteractionObject = interaction;
                        if (currentInteractionObject != null && currentInteractionObject != interaction)
                        {
                            currentInteractionObject = interaction;
                            if (currentInteractionObject.IsAutoInteractable)
                            {
                                interaction.Interact(this);
                            }
                        }

                    }
                    else if (currentInteractionObject == null)
                    {
                        currentInteractionObject = interaction;
                        if (currentInteractionObject.IsAutoInteractable)
                        {
                            interaction.Interact(this);
                        }
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

