using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace April
{
    public class IngameInputSystem : MonoBehaviour
    {
        public GameObject escapeUI;
        public GameObject player;

        public float playerSpeed = 5f;
        public float playerTurnSmoothTime;

        private CharacterController characterController;
        private Vector2 movementInput;
        private float playerTurningCurrentVelocity;


        private void Awake()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Enable();
            InputManager.Singleton.InputMaster.PlayerControl.Escaping.performed += Escaping_performed;
            InputManager.Singleton.InputMaster.PlayerControl.Movement.performed += OnMovementPerform;
            InputManager.Singleton.InputMaster.PlayerControl.Movement.canceled += OnMovementCanceled;

            characterController = player.GetComponent<CharacterController>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            movementInput = Vector2.zero;
        }

        private void OnMovementPerform(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            if (movementInput.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(characterController.transform.eulerAngles.y, targetAngle, ref playerTurningCurrentVelocity, playerTurnSmoothTime);
                characterController.transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                characterController.Move(moveDir.normalized * playerSpeed * Time.deltaTime);
            }
        }

        private void Escaping_performed(InputAction.CallbackContext context)
        {
            Debug.Log(context);
            if (context.performed)
            {
                escapeUI.SetActive(!escapeUI.activeSelf);
            }
        }

        public void GoToTitleScene()
        {
            Main.Singleton.ChangeScene(SceneType.Title);
        }
    }
}

