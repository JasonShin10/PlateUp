using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

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

        public float interactionOffsetHeight = 0.8f;

        public LayerMask interactionObjectLayerMask;

        [Title("Settings")]
        [field: SerializeField] public PlayerData RuntimePlayerData { get; set; }
        public float playerTurnSmoothTime;

        [Title("UI")]
        public string playerName;
        public TMPro.TextMeshProUGUI playerNameText;

        [Title("Visualization")]
        public VisualizationCharacter visualization;

        private CharacterController characterController;
        private bool isMouseOverGUI;
        private float playerTurningCurrentVelocity;

        private InteractionBase interactionBase;
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
            InputManager.Singleton.InputMaster.PlayerControl.HoldInteract.performed += HoldInteraction;
            InputManager.Singleton.InputMaster.PlayerControl.HoldInteract.canceled += StopInteraction;
            InputManager.Singleton.InputMaster.PlayerControl.Interact.canceled += StopInteraction;

            IngameLifeSystem.Instance.OnLifeCountChanged += OnLifeChanged;
            IngameEndSystem.Instance.OnGameCleared += OnGameCleared;

            //InputManager.Singleton.InputMaster.PlayerControl.Click.performed += MouseClick;
            //InputManager.Singleton.InputMaster.PlayerControl.CursorEnable.performed += CursorEnable;


            playerNameText.text = playerName;
        }

        private void OnDisable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Interact.performed -= DoInteraction;
            InputManager.Singleton.InputMaster.PlayerControl.HoldInteract.performed -= HoldInteraction;
            InputManager.Singleton.InputMaster.PlayerControl.HoldInteract.canceled -= StopInteraction;
            InputManager.Singleton.InputMaster.PlayerControl.Interact.canceled -= StopInteraction;

            if (IngameLifeSystem.Instance)
            {
                IngameLifeSystem.Instance.OnLifeCountChanged -= OnLifeChanged;
            }
            if (IngameEndSystem.Instance)
            {
                IngameEndSystem.Instance.OnGameCleared -= OnGameCleared;
            }

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
               
            }
        }

        private void DeactivateInteraction_Animation(InteractionBase currentInteraction)
        {
            switch (currentInteraction)
            {
                case FoodContainer _:
                    this.visualization.SetInteractionFoodContainer(false);
                    break;
               
            }
        }
        private void DoInteraction(InputAction.CallbackContext context)
        {

            currentInteractionObject?.Interact(this);

        }

        private void HoldInteraction(InputAction.CallbackContext context)
        {

            isButtonPressed = true;
            if (currentInteractionObject != null)
            {
            this.visualization.SetInteractionCook(true);

            }
            if (currentInteractionObject is DishWasher)
            {
                var dishWasher = (DishWasher)currentInteractionObject;
                dishWasher.particleController.PlayParticle();
            }

        }

        private void StopInteraction(InputAction.CallbackContext context)
        {

            isButtonPressed = false;
            this.visualization.SetInteractionCook(false);
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
                        
                        if (currentInteractionObject is InteractionBase interactionBase)
                        {
                            this.interactionBase?.TakeOutOutLineMaterial();
                            this.interactionBase = interactionBase;
                            this.interactionBase?.PutOutLineMaterial();
                            if (interactionBase.IsAutoInteractable)
                            {

                                interactionBase.Interact(this);
                                ActivateInteraction_Animation(interactionBase);

                            }
                        }
                        else if (currentInteractionObject is CharacterBase interactionCharacter)
                        {
                           
                            if (interactionCharacter.IsAutoInteractable)
                            {
                                interactionCharacter.Interact(this);

                            }
                        }
                    }
                }
                else
                {
                    this.interactionBase?.TakeOutOutLineMaterial();
                }
            }
            else
            {
                this.interactionBase?.TakeOutOutLineMaterial();
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
            if (false == IngameLifeSystem.Instance.HasRemainLife)
                return;

            float targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(characterController.transform.eulerAngles.y, targetAngle, ref playerTurningCurrentVelocity, playerTurnSmoothTime);
            characterController.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * RuntimePlayerData.PlayerSpeed * Time.deltaTime);

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

        private void OnLifeChanged(int remainLife)
        {
            IngameUI.Instance.SetLife(remainLife);

            // Game Over
            if (remainLife <= 0)
            {
                UIManager.Show<GameOverUI>(UIList.GameOverUI);

                IngameCameraSystem.Instance.ChangeCamera(CameraModeType.Camera_GameOver);

                IngameTimeSystem.Instance.SetTimeScale(1f);

                IngameTimeSystem.Instance.IsUpdateEnable = false;

                IngameCustomerFactorySystem.Instance.enabled = false;

                visualization.SetGameOver(true);
                //visualization.SetGameOverAnimation(true);
            }
        }

        private void OnGameCleared()
        {
            IngameCameraSystem.Instance.ChangeCamera(CameraModeType.Camera_GameOver);
            IngameTimeSystem.Instance.SetTimeScale(1f);
            IngameTimeSystem.Instance.IsUpdateEnable = false;
            IngameCustomerFactorySystem.Instance.enabled = false;
            visualization.SetVictory(true);
        }

    }
}

