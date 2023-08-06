using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.UI;
using TMPro;
using System.Xml.Serialization;
using System;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace April
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        public bool isButtonPressed = false;
        public bool runButtonPressed = false;
        [Title("Components")]
        public InteractionBase currentInteractionObject;
        public InteractionItem item;

        public float interactionOffsetHeight = 0.8f;
        public LayerMask interactionObjectLayerMask;

        [Title("Settings")]
        public float playerSpeed = 5f;
        public float playerTurnSmoothTime;

        [Title("UI")]
        public string playerName;
        public TMPro.TextMeshProUGUI playerNameText;

        [Title("Visualization")]
        public VisualizationCharacter visualization;

        private CharacterController characterController;
        private bool isMouseOverGUI;
        private float playerTurningCurrentVelocity;

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
            InputManager.Singleton.InputMaster.PlayerControl.Interact.canceled += StopInteraction;
            InputManager.Singleton.InputMaster.PlayerControl.Click.performed += MouseClick;
            InputManager.Singleton.InputMaster.PlayerControl.CursorEnable.performed += CursorEnable;


            playerNameText.text = playerName;
        }

        private void OnDisable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Interact.performed -= DoInteraction;
            InputManager.Singleton.InputMaster.PlayerControl.Click.performed -= MouseClick;
            InputManager.Singleton.InputMaster.PlayerControl.CursorEnable.performed -= CursorEnable;
        }

        private void MouseClick(InputAction.CallbackContext context)
        {
            if (!isMouseOverGUI)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void CursorEnable(InputAction.CallbackContext context)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void DoInteraction(InputAction.CallbackContext context)
        {
            Debug.Log("DoInteraction");
            currentInteractionObject?.Interact(this);
            isButtonPressed = true;
        }

        private void StopInteraction(InputAction.CallbackContext context)
        {
            Debug.Log("StopInteraction");
            isButtonPressed = false;
        }

        bool IsMouseOverGameWindow()
        {
            Vector3 mousePosition = Input.mousePosition;
            return mousePosition.x >= 0 && mousePosition.x <= Screen.width && mousePosition.y >= 0 && mousePosition.y <= Screen.height;
        }

        private void Update()
        {
            if (EventSystem.current != null)
                isMouseOverGUI = EventSystem.current.IsPointerOverGameObject();
            else
                isMouseOverGUI = false;

            //if (IsMouseOverGameWindow())
            //{
            //    Cursor.visible = false;
            //}
            //else
            //{
            //    Cursor.visible = true;
            //}

            // Ray의 시작점을 플레이어의 발 아래가 아니라, 약간 위에서 시작하게 하는것
            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out var hitInfo, 1.5f, interactionObjectLayerMask, QueryTriggerInteraction.Collide))
            {
                if (hitInfo.transform.TryGetComponent(out InteractionBase interaction))
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

        public void Move(Vector2 movementInput)
        {
            float targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(characterController.transform.eulerAngles.y, targetAngle, ref playerTurningCurrentVelocity, playerTurnSmoothTime);
            characterController.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * playerSpeed * Time.deltaTime);

            visualization.SetMovement(movementInput.magnitude);
        }

        public void MoveStop()
        {
            visualization.SetMovement(0);
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

