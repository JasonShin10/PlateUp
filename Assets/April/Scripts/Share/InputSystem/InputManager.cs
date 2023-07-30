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
        public InputMaster InputMaster { get; private set; }

        public Vector3 MouseScreenPosition { get => Mouse.current.position.ReadValue(); }
        public Vector3 MouseViewportPosition { get => Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue()); }

        protected override void Awake()
        {
            base.Awake();
            InputMaster = new InputMaster();
        }
    }
}

