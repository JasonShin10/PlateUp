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

        private Rigidbody capshuleRigidbody;

        private void Awake()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Enable();
            InputManager.Singleton.InputMaster.PlayerControl.Escaping.performed += Escaping_performed;
            capshuleRigidbody = player.GetComponent<Rigidbody>();

            InputManager.Singleton.InputMaster.PlayerControl.Movement.performed += OnMovementPerform;
        }

        private void OnMovementPerform(InputAction.CallbackContext context)
        {
            var movement = context.ReadValue<Vector2>();
            Debug.Log(movement);
        }

        private void Update()
        {
            Vector2 inputVector = InputManager.Singleton.InputMaster.PlayerControl.Movement.ReadValue<Vector2>();
            float speed = 1f;
            capshuleRigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
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

