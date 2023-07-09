using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.UI;

namespace April
{
    public class PlayerController : MonoBehaviour
    {
        #region v1
        //public float speed;
        //private Vector2 move;

        //public void OnMove(InputAction.CallbackContext context)
        //{

        //    move = context.ReadValue<Vector2>();
        //    new InputMaster(); 
        //}

        //private void Update()
        //{
        //    movePlayer();
        //}

        //public void movePlayer()
        //{
        //    Vector3 movement = new Vector3(move.x, 0f, move.y);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        //    var pos = transform.position + movement * speed * Time.deltaTime;

        //    transform.position = pos;
        //}
        #endregion
        private Rigidbody capshuleRigidbody;
        private InputMaster inputMaster;
        public GameObject escapeUI;
        private void Awake()
        {
            capshuleRigidbody = GetComponent<Rigidbody>();


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

            }
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

