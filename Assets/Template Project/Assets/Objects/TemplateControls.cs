// GENERATED AUTOMATICALLY FROM 'Assets/Template Project/Assets/Objects/Template PC Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace TemplateGame
{
    public class @TemplateControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @TemplateControls()
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
                    ""type"": ""Value"",
                    ""id"": ""f0c62bb3-b4ba-4072-bd58-9ae8b560e892"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""41d52165-f620-419e-b3d2-5d002146ce0f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""75c20af1-8e07-44d1-8883-80fc13a39b69"",
                    ""expectedControlType"": ""Vector2"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""cf24a6fa-9460-460c-ac17-3929d119898e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d045b812-3ce6-470e-bee4-05b5580b83a6"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
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
            m_Newactionmap_Jump = m_Newactionmap.FindAction("Jump", throwIfNotFound: true);
            m_Newactionmap_Look = m_Newactionmap.FindAction("Look", throwIfNotFound: true);
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
        private readonly InputAction m_Newactionmap_Jump;
        private readonly InputAction m_Newactionmap_Look;
        public struct NewactionmapActions
        {
            private @TemplateControls m_Wrapper;
            public NewactionmapActions(@TemplateControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Left => m_Wrapper.m_Newactionmap_Left;
            public InputAction @Right => m_Wrapper.m_Newactionmap_Right;
            public InputAction @Jump => m_Wrapper.m_Newactionmap_Jump;
            public InputAction @Look => m_Wrapper.m_Newactionmap_Look;
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
                    @Jump.started -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnJump;
                    @Look.started -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnLook;
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
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
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
            void OnJump(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
        }
    }
}
