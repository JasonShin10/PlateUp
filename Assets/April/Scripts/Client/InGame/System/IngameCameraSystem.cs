using Cinemachine;
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
    }

    public class IngameCameraSystem : MonoBehaviour
    {
        public static IngameCameraSystem Instance { get; private set; }

        [Sirenix.OdinInspector.Title("Camera Settings")]
        [SerializeField] private CameraModeType currentCameraMode = CameraModeType.Camera_PlayerFocus;


        [Sirenix.OdinInspector.Title("Cameras")]
        [SerializeField]
        private SerializableDictionary<CameraModeType, CinemachineVirtualCameraBase> Cameras
            = new SerializableDictionary<CameraModeType, CinemachineVirtualCameraBase>();


        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        [Sirenix.OdinInspector.Button("Change Camera")]
        public void ChangeCamera(CameraModeType cameraMode)
        {
            if (currentCameraMode == cameraMode)
                return;

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

