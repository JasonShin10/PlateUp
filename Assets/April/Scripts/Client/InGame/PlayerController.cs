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
        public Food item;
        public Dish dish;
        public float interactionOffsetHeight = 0.8f;
        public LayerMask interactionObjectLayerMask;

        public string playerName;
        public TMPro.TextMeshProUGUI playerNameText;

        private CharacterController characterController;

        private void Awake()
        {
            Instance = this;
            characterController = GetComponent<CharacterController>();
            
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
        bool IsMouseOverGameWindow()
        {
            Vector3 mousePosition = Input.mousePosition;

            return mousePosition.x >= 0 && mousePosition.x <= Screen.width && mousePosition.y >= 0 && mousePosition.y <= Screen.height;
        }
        private void Update()
        {
            if (IsMouseOverGameWindow())
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
            // Ray의 시작점을 플레이어의 발 아래가 아니라, 약간 위에서 시작하게 하는것
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out var hitInfo, 1f, interactionObjectLayerMask, QueryTriggerInteraction.Collide))
            {
                if (hitInfo.transform.TryGetComponent<InteractionBase>(out var interaction))
                {
                    // stove를 바라보면 currentInteractionObject는 stove. 
                    // 다른데 바라볼때 다른게 갱신되게끔 하는건가?
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


            if (!characterController.isGrounded)
            {
                Vector3 gravity = Physics.gravity;
                characterController.Move(gravity * Time.deltaTime);
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

