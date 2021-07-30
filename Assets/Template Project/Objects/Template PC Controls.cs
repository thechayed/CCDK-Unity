// GENERATED AUTOMATICALLY FROM 'Assets/Template Project/Objects/Template PC Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TemplatePCControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TemplatePCControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Template PC Controls"",
    ""maps"": [
        {
            ""name"": ""New action map"",
            ""id"": ""644c2907-e3df-435c-b6e1-ebafd005cc3e"",
            ""actions"": [
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""cd390b28-9463-41a4-939b-63e32f221e89"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""f0c62bb3-b4ba-4072-bd58-9ae8b560e892"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""60b81ef3-15ce-421e-ba92-b96ce6cd8146"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=-1)"",
                    ""groups"": ""New control scheme"",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6edbfd7-d3b3-460b-8af1-1d6f9a1dc8db"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""New control scheme"",
            ""bindingGroup"": ""New control scheme"",
            ""devices"": []
        }
    ]
}");
        // New action map
        m_Newactionmap = asset.FindActionMap("New action map", throwIfNotFound: true);
        m_Newactionmap_Left = m_Newactionmap.FindAction("Left", throwIfNotFound: true);
        m_Newactionmap_Right = m_Newactionmap.FindAction("Right", throwIfNotFound: true);
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

    // New action map
    private readonly InputActionMap m_Newactionmap;
    private INewactionmapActions m_NewactionmapActionsCallbackInterface;
    private readonly InputAction m_Newactionmap_Left;
    private readonly InputAction m_Newactionmap_Right;
    public struct NewactionmapActions
    {
        private @TemplatePCControls m_Wrapper;
        public NewactionmapActions(@TemplatePCControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Left => m_Wrapper.m_Newactionmap_Left;
        public InputAction @Right => m_Wrapper.m_Newactionmap_Right;
        public InputActionMap Get() { return m_Wrapper.m_Newactionmap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NewactionmapActions set) { return set.Get(); }
        public void SetCallbacks(INewactionmapActions instance)
        {
            if (m_Wrapper.m_NewactionmapActionsCallbackInterface != null)
            {
                @Left.started -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnLeft;
                @Left.performed -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnLeft;
                @Left.canceled -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnLeft;
                @Right.started -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnRight;
                @Right.performed -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnRight;
                @Right.canceled -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnRight;
            }
            m_Wrapper.m_NewactionmapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Left.started += instance.OnLeft;
                @Left.performed += instance.OnLeft;
                @Left.canceled += instance.OnLeft;
                @Right.started += instance.OnRight;
                @Right.performed += instance.OnRight;
                @Right.canceled += instance.OnRight;
            }
        }
    }
    public NewactionmapActions @Newactionmap => new NewactionmapActions(this);
    private int m_NewcontrolschemeSchemeIndex = -1;
    public InputControlScheme NewcontrolschemeScheme
    {
        get
        {
            if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
            return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
        }
    }
    public interface INewactionmapActions
    {
        void OnLeft(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
    }
}
