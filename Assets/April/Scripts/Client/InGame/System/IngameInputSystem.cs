using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
            InputManager.Singleton.InputMaster.PlayerControl.Run.performed += DoRun;
            InputManager.Singleton.InputMaster.PlayerControl.Run.canceled += StopRun;
        }


        private void DoRun(InputAction.CallbackContext context)
        {
            player.playerSpeed *= 2;
        }

        private void StopRun(InputAction.CallbackContext context)
        {
            player.playerSpeed /= 2;
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

