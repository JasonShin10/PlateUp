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
        public PlayerController player;

        private Vector2 movementInput;

        private void Awake()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Enable();
            InputManager.Singleton.InputMaster.PlayerControl.Escaping.performed += Escaping_performed;
            InputManager.Singleton.InputMaster.PlayerControl.Movement.performed += OnMovementPerform;
            InputManager.Singleton.InputMaster.PlayerControl.Movement.canceled += OnMovementCanceled;
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
                player.Move(movementInput);
            }
            else
            {
                player.MoveStop();
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

