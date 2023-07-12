//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/April/Scripts/Share/InputSystem/InputMaster.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputMaster: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""PlayerControl"",
            ""id"": ""119e335e-63ed-4d50-9b13-0d1b13329ef8"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6041b18c-7e9c-40e1-af03-52ac63dbc66d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Escaping"",
                    ""type"": ""Button"",
                    ""id"": ""07dafce2-2f26-483d-a5b6-ce1c019e17d9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""f5e8956e-a834-4b51-b4c6-acf7e20eabc0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3bdad9f9-a560-434c-82cf-2635038bfe3e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""731082b1-ddaa-472a-8d76-f1b2f1ea09e1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4952aacc-e9ef-44e6-aabf-26502796388e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""15e5c8b2-bf2a-423b-aed2-80c80b1bf511"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0226faea-3f55-43b6-98fc-a75491042fb2"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escaping"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI Control"",
            ""id"": ""f1c8f31d-3230-4324-80cd-e5850b6d7eed"",
            ""actions"": [
                {
                    ""name"": ""InteractionShortcut"",
                    ""type"": ""Button"",
                    ""id"": ""cb351774-1a46-4479-b7d4-68cb823b8b37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""28a59235-1045-4a54-acf3-e610080c8199"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InteractionShortcut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerControl
        m_PlayerControl = asset.FindActionMap("PlayerControl", throwIfNotFound: true);
        m_PlayerControl_Movement = m_PlayerControl.FindAction("Movement", throwIfNotFound: true);
        m_PlayerControl_Escaping = m_PlayerControl.FindAction("Escaping", throwIfNotFound: true);
        // UI Control
        m_UIControl = asset.FindActionMap("UI Control", throwIfNotFound: true);
        m_UIControl_InteractionShortcut = m_UIControl.FindAction("InteractionShortcut", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerControl
    private readonly InputActionMap m_PlayerControl;
    private List<IPlayerControlActions> m_PlayerControlActionsCallbackInterfaces = new List<IPlayerControlActions>();
    private readonly InputAction m_PlayerControl_Movement;
    private readonly InputAction m_PlayerControl_Escaping;
    public struct PlayerControlActions
    {
        private @InputMaster m_Wrapper;
        public PlayerControlActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerControl_Movement;
        public InputAction @Escaping => m_Wrapper.m_PlayerControl_Escaping;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerControlActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerControlActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerControlActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Escaping.started += instance.OnEscaping;
            @Escaping.performed += instance.OnEscaping;
            @Escaping.canceled += instance.OnEscaping;
        }

        private void UnregisterCallbacks(IPlayerControlActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Escaping.started -= instance.OnEscaping;
            @Escaping.performed -= instance.OnEscaping;
            @Escaping.canceled -= instance.OnEscaping;
        }

        public void RemoveCallbacks(IPlayerControlActions instance)
        {
            if (m_Wrapper.m_PlayerControlActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerControlActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerControlActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerControlActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerControlActions @PlayerControl => new PlayerControlActions(this);

    // UI Control
    private readonly InputActionMap m_UIControl;
    private List<IUIControlActions> m_UIControlActionsCallbackInterfaces = new List<IUIControlActions>();
    private readonly InputAction m_UIControl_InteractionShortcut;
    public struct UIControlActions
    {
        private @InputMaster m_Wrapper;
        public UIControlActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @InteractionShortcut => m_Wrapper.m_UIControl_InteractionShortcut;
        public InputActionMap Get() { return m_Wrapper.m_UIControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIControlActions set) { return set.Get(); }
        public void AddCallbacks(IUIControlActions instance)
        {
            if (instance == null || m_Wrapper.m_UIControlActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIControlActionsCallbackInterfaces.Add(instance);
            @InteractionShortcut.started += instance.OnInteractionShortcut;
            @InteractionShortcut.performed += instance.OnInteractionShortcut;
            @InteractionShortcut.canceled += instance.OnInteractionShortcut;
        }

        private void UnregisterCallbacks(IUIControlActions instance)
        {
            @InteractionShortcut.started -= instance.OnInteractionShortcut;
            @InteractionShortcut.performed -= instance.OnInteractionShortcut;
            @InteractionShortcut.canceled -= instance.OnInteractionShortcut;
        }

        public void RemoveCallbacks(IUIControlActions instance)
        {
            if (m_Wrapper.m_UIControlActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIControlActions instance)
        {
            foreach (var item in m_Wrapper.m_UIControlActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIControlActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIControlActions @UIControl => new UIControlActions(this);
    public interface IPlayerControlActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnEscaping(InputAction.CallbackContext context);
    }
    public interface IUIControlActions
    {
        void OnInteractionShortcut(InputAction.CallbackContext context);
    }
}
