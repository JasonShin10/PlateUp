using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public enum CameraModeType
    {
        None = 0,

        Camera_PlayerFocus,
        Camera_TopView,
        Camera_GameOver,
    }

    public class IngameCameraSystem : MonoBehaviour
    {
        public static IngameCameraSystem Instance { get; private set; }

        [Sirenix.OdinInspector.Title("Camera Settings")]
        [SerializeField] private CameraModeType currentCameraMode = CameraModeType.Camera_TopView;


        [Sirenix.OdinInspector.Title("Cameras")]
        [SerializeField]
        private SerializableDictionary<CameraModeType, CinemachineVirtualCameraBase> Cameras
            = new SerializableDictionary<CameraModeType, CinemachineVirtualCameraBase>();


        [SerializeField] private Cinemachine.CinemachineFreeLook playerFocusCamera;

        [SerializeField] private float horizontalRotateSpeed = 1;
        [SerializeField] private float verticalRotateSpeed = 1;

        private float horizontalRotateValue;
        private float verticalRotateValue;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InputManager.Singleton.InputMaster.PlayerControl.CameraRotateHorizontal.performed += CameraRotateHorizontal;
            InputManager.Singleton.InputMaster.PlayerControl.CameraRotateVertical.performed += CameraRotateVertical;

            ChangeCamera(CameraModeType.Camera_TopView);
        }


        private void CameraRotateHorizontal(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            var deltaX = context.ReadValue<float>();
            horizontalRotateValue = deltaX;
        }

        private void CameraRotateVertical(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            var deltaY = context.ReadValue<float>();
            verticalRotateValue = deltaY;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void LateUpdate()
        {
            switch (currentCameraMode)
            {
                case CameraModeType.Camera_PlayerFocus:
                    {
                        playerFocusCamera.m_XAxis.Value += horizontalRotateValue * horizontalRotateSpeed * Time.deltaTime;
                        //playerFocusCamera.m_YAxis.Value += verticalRotateValue * verticalRotateSpeed * Time.deltaTime;
                    }
                    break;
            }
        }



        //[Sirenix.OdinInspector.Button("Change Camera")]
        public void ChangeCamera(CameraModeType cameraMode)
        {
            if (currentCameraMode == cameraMode)
            {

                return;
            }

            if (Cameras.TryGetValue(currentCameraMode, out var currentCamera))
            {

                currentCamera.gameObject.SetActive(false);
            }


            if (Cameras.TryGetValue(cameraMode, out var targetCamera))
            {

                targetCamera.gameObject.SetActive(true);
            }


            currentCameraMode = cameraMode;
        }
    }
}

