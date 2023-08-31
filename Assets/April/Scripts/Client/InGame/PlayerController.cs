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
using System.Globalization;

namespace April
{
    public class PlayerController : CharacterBase
    {
        public static PlayerController Instance { get; private set; }
        public override CharacterType CharacterType
        {
            get
            {
                return CharacterType.Player;
            }
        }
        public bool isButtonPressed = false;
        public bool runButtonPressed = false;
        [Title("Components")]
        public IRaycastInterface currentInteractionObject;
        //public CharacterBase currentInteracionCharacter;
        //public IRaycastInterface currentInteraction;
        

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

        protected override void Awake()
        {
            Instance = this;
            characterController = GetComponent<CharacterController>();
        }

        protected override void OnDestroy()
        {
            Instance = null;
        }

        public override void Start()
        {
            
        }
        private void OnEnable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Interact.performed += DoInteraction;
            InputManager.Singleton.InputMaster.PlayerControl.Interact.canceled += StopInteraction;
            //InputManager.Singleton.InputMaster.PlayerControl.Click.performed += MouseClick;
            //InputManager.Singleton.InputMaster.PlayerControl.CursorEnable.performed += CursorEnable;


            playerNameText.text = playerName;
        }

        private void OnDisable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Interact.performed -= DoInteraction;
            //InputManager.Singleton.InputMaster.PlayerControl.Click.performed -= MouseClick;
            //InputManager.Singleton.InputMaster.PlayerControl.CursorEnable.performed -= CursorEnable;
        }

        //private void MouseClick(InputAction.CallbackContext context)
        //{
        //    if (!isMouseOverGUI)
        //    {
        //        Cursor.visible = false;
        //        Cursor.lockState = CursorLockMode.Locked;
        //    }
        //}

        //private void CursorEnable(InputAction.CallbackContext context)
        //{
        //    Cursor.visible = true;
        //    Cursor.lockState = CursorLockMode.None;
        //}

        private void ActivateInteraction_Animation(InteractionBase currentInteraction)
        {
            switch (currentInteraction)
            {
                case FoodContainer _:
                    this.visualization.SetInteractionFoodContainer(true);
                    break;
                case DishWasher _:
                    this.visualization.SetInteractionCook(true);
                    break;
            }
        }

        private void DeactivateInteraction_Animation(InteractionBase currentInteraction)
        {
            switch (currentInteraction)
            {
                case FoodContainer _:
                    this.visualization.SetInteractionFoodContainer(false);
                    break;
                case DishWasher _:
                    this.visualization.SetInteractionCook(false);
                    break;
            }
        }
        private void DoInteraction(InputAction.CallbackContext context)
        {
            Debug.Log("DoInteraction");
            currentInteractionObject?.Interact(this);
            isButtonPressed = true;
            if (currentInteractionObject is DishWasher)
            {
                var dishWasher = (DishWasher)currentInteractionObject;
                dishWasher.particleController.PlayParticle();
            }
        }

        private void StopInteraction(InputAction.CallbackContext context)
        {
            Debug.Log("StopInteraction");
            isButtonPressed = false;
            if (currentInteractionObject != null)
            {
                DeactivateInteraction_Animation(currentInteractionObject as InteractionBase);
            }
            if (currentInteractionObject is DishWasher)
            {
                var dishWasher = (DishWasher)currentInteractionObject;
                dishWasher.particleController.StopParticle();
            }
        }

        bool IsMouseOverGameWindow()
        {
            Vector3 mousePosition = Input.mousePosition;
            return mousePosition.x >= 0 && mousePosition.x <= Screen.width && mousePosition.y >= 0 && mousePosition.y <= Screen.height;
        }

        protected override void Update()
        {
            if (EventSystem.current != null)
                isMouseOverGUI = EventSystem.current.IsPointerOverGameObject();
            else
                isMouseOverGUI = false;

            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out var hitInfo, 1.5f, interactionObjectLayerMask, QueryTriggerInteraction.Collide))
            {
                bool FoundInteractionObject = hitInfo.transform.TryGetComponent(out IRaycastInterface interaction);
           
                if (FoundInteractionObject)
                {
 
                    if (currentInteractionObject != interaction)
                    {                     
                            
                        currentInteractionObject = interaction;
                        if (interaction is InteractionBase interactionBase)
                        {
                            if (interactionBase.IsAutoInteractable)
                            {
                                interactionBase.Interact(this);
                                ActivateInteraction_Animation(interactionBase);
                            }                     
                   }
                        else if (interaction is CharacterBase interactionCharacter)
                        {
                            if (interactionCharacter.IsAutoInteractable)
                            {
                                interactionCharacter.Interact(this);
                               
                            }
                        }
                    }
                }
            }
            else
            {
                UIManager.Hide<InteractionUI>(UIList.InteractionUI);
                DeactivateInteraction_Animation(currentInteractionObject as InteractionBase);
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

