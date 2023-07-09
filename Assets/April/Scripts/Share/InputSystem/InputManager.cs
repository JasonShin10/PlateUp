using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace April
{
    public class InputManager : SingletonBase<InputManager>
    {
        private Rigidbody capshuleRigidbody;
        private InputMaster inputMaster;
        public GameObject escapeUI;
        public GameObject player;

        protected override void Awake()
        {
            base.Awake();
            capshuleRigidbody = player.GetComponent<Rigidbody>();
            inputMaster = new InputMaster();
            inputMaster.PlayerControl.Enable();
            inputMaster.PlayerControl.Escaping.performed += Escaping_performed;
        }

        private void Update()
        {
            Vector2 inputVector = inputMaster.PlayerControl.Movement.ReadValue<Vector2>();
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

        private void Movement_performed(InputAction.CallbackContext context)
        {
            Debug.Log(context);
            Vector2 inputVector = context.ReadValue<Vector2>();
            float speed = 5f;
            capshuleRigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
        }
    }
}

