// GENERATED AUTOMATICALLY FROM 'Assets/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Paddle"",
            ""id"": ""c1626425-5569-4a46-882d-03e7e51ed0dc"",
            ""actions"": [
                {
                    ""name"": ""MoveP1"",
                    ""type"": ""PassThrough"",
                    ""id"": ""97324925-b0e1-4fd1-a005-662f6c8e0824"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveP2"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9f9f474c-5a96-4286-beef-2e89ae91f1f7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SprintP1"",
                    ""type"": ""PassThrough"",
                    ""id"": ""89614321-4d5b-40f1-b2c6-8109c01dc3f8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SprintP2"",
                    ""type"": ""Button"",
                    ""id"": ""e03bef3c-ef3b-4b91-ae16-24772eaa8755"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""0412e681-3cbb-4be4-89b1-93444df6d227"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP1"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""393ad0c7-ce59-4631-a862-a5623c01b2c5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d080a7da-60e3-41ee-a84f-3a474cab34fd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c190f410-b3c7-443a-a29b-ab2fa8c18bcb"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d6fd1796-2560-4576-8392-a23ca5bc01f9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""21c1b09b-3c4e-4a12-a07d-1fc76bd1e4e7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d858fc2f-304a-463b-8a39-3a6997566573"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b4d6da5a-1c28-4a00-ab48-5b633adfc290"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""dfc0a5e6-e8fd-4e09-9761-09a86ea5b926"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c78ea898-ae31-4e7f-b7f3-73421d2150d4"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""c115dec1-be1e-4205-b6b0-0a6dd47bf096"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SprintP1"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""209143ad-972d-4b79-938e-4ff4be5af59c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SprintP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""50407153-92fc-4845-aae2-e934126ae132"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SprintP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""23482bfa-6f03-4885-afd3-2530a244d42d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SprintP2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""7a6e943d-0170-4b5e-9ebf-4ba7987f3030"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SprintP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""55ff6a84-dedf-489f-aef0-21858de62796"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SprintP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""NoteListener"",
            ""id"": ""75c7cf47-23eb-4ced-b5cc-2157109c1c3c"",
            ""actions"": [
                {
                    ""name"": ""MousePos"",
                    ""type"": ""Value"",
                    ""id"": ""e8b5d1c9-7ee3-48a7-8253-a27e5814bd33"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""efc2faab-c735-4a69-9745-b8b8d4c660dc"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debugger"",
            ""id"": ""460c110b-0ac4-48de-9559-5606e1d64fbe"",
            ""actions"": [
                {
                    ""name"": ""ToggleDebugLines"",
                    ""type"": ""Button"",
                    ""id"": ""afea4c5d-4f28-4d68-b173-9561875c0e90"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3a710299-7100-4591-8fca-64b4f0447470"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ToggleDebugLines"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Paddle
        m_Paddle = asset.FindActionMap("Paddle", throwIfNotFound: true);
        m_Paddle_MoveP1 = m_Paddle.FindAction("MoveP1", throwIfNotFound: true);
        m_Paddle_MoveP2 = m_Paddle.FindAction("MoveP2", throwIfNotFound: true);
        m_Paddle_SprintP1 = m_Paddle.FindAction("SprintP1", throwIfNotFound: true);
        m_Paddle_SprintP2 = m_Paddle.FindAction("SprintP2", throwIfNotFound: true);
        // NoteListener
        m_NoteListener = asset.FindActionMap("NoteListener", throwIfNotFound: true);
        m_NoteListener_MousePos = m_NoteListener.FindAction("MousePos", throwIfNotFound: true);
        // Debugger
        m_Debugger = asset.FindActionMap("Debugger", throwIfNotFound: true);
        m_Debugger_ToggleDebugLines = m_Debugger.FindAction("ToggleDebugLines", throwIfNotFound: true);
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

    // Paddle
    private readonly InputActionMap m_Paddle;
    private IPaddleActions m_PaddleActionsCallbackInterface;
    private readonly InputAction m_Paddle_MoveP1;
    private readonly InputAction m_Paddle_MoveP2;
    private readonly InputAction m_Paddle_SprintP1;
    private readonly InputAction m_Paddle_SprintP2;
    public struct PaddleActions
    {
        private @InputMaster m_Wrapper;
        public PaddleActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveP1 => m_Wrapper.m_Paddle_MoveP1;
        public InputAction @MoveP2 => m_Wrapper.m_Paddle_MoveP2;
        public InputAction @SprintP1 => m_Wrapper.m_Paddle_SprintP1;
        public InputAction @SprintP2 => m_Wrapper.m_Paddle_SprintP2;
        public InputActionMap Get() { return m_Wrapper.m_Paddle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PaddleActions set) { return set.Get(); }
        public void SetCallbacks(IPaddleActions instance)
        {
            if (m_Wrapper.m_PaddleActionsCallbackInterface != null)
            {
                @MoveP1.started -= m_Wrapper.m_PaddleActionsCallbackInterface.OnMoveP1;
                @MoveP1.performed -= m_Wrapper.m_PaddleActionsCallbackInterface.OnMoveP1;
                @MoveP1.canceled -= m_Wrapper.m_PaddleActionsCallbackInterface.OnMoveP1;
                @MoveP2.started -= m_Wrapper.m_PaddleActionsCallbackInterface.OnMoveP2;
                @MoveP2.performed -= m_Wrapper.m_PaddleActionsCallbackInterface.OnMoveP2;
                @MoveP2.canceled -= m_Wrapper.m_PaddleActionsCallbackInterface.OnMoveP2;
                @SprintP1.started -= m_Wrapper.m_PaddleActionsCallbackInterface.OnSprintP1;
                @SprintP1.performed -= m_Wrapper.m_PaddleActionsCallbackInterface.OnSprintP1;
                @SprintP1.canceled -= m_Wrapper.m_PaddleActionsCallbackInterface.OnSprintP1;
                @SprintP2.started -= m_Wrapper.m_PaddleActionsCallbackInterface.OnSprintP2;
                @SprintP2.performed -= m_Wrapper.m_PaddleActionsCallbackInterface.OnSprintP2;
                @SprintP2.canceled -= m_Wrapper.m_PaddleActionsCallbackInterface.OnSprintP2;
            }
            m_Wrapper.m_PaddleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveP1.started += instance.OnMoveP1;
                @MoveP1.performed += instance.OnMoveP1;
                @MoveP1.canceled += instance.OnMoveP1;
                @MoveP2.started += instance.OnMoveP2;
                @MoveP2.performed += instance.OnMoveP2;
                @MoveP2.canceled += instance.OnMoveP2;
                @SprintP1.started += instance.OnSprintP1;
                @SprintP1.performed += instance.OnSprintP1;
                @SprintP1.canceled += instance.OnSprintP1;
                @SprintP2.started += instance.OnSprintP2;
                @SprintP2.performed += instance.OnSprintP2;
                @SprintP2.canceled += instance.OnSprintP2;
            }
        }
    }
    public PaddleActions @Paddle => new PaddleActions(this);

    // NoteListener
    private readonly InputActionMap m_NoteListener;
    private INoteListenerActions m_NoteListenerActionsCallbackInterface;
    private readonly InputAction m_NoteListener_MousePos;
    public struct NoteListenerActions
    {
        private @InputMaster m_Wrapper;
        public NoteListenerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePos => m_Wrapper.m_NoteListener_MousePos;
        public InputActionMap Get() { return m_Wrapper.m_NoteListener; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NoteListenerActions set) { return set.Get(); }
        public void SetCallbacks(INoteListenerActions instance)
        {
            if (m_Wrapper.m_NoteListenerActionsCallbackInterface != null)
            {
                @MousePos.started -= m_Wrapper.m_NoteListenerActionsCallbackInterface.OnMousePos;
                @MousePos.performed -= m_Wrapper.m_NoteListenerActionsCallbackInterface.OnMousePos;
                @MousePos.canceled -= m_Wrapper.m_NoteListenerActionsCallbackInterface.OnMousePos;
            }
            m_Wrapper.m_NoteListenerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePos.started += instance.OnMousePos;
                @MousePos.performed += instance.OnMousePos;
                @MousePos.canceled += instance.OnMousePos;
            }
        }
    }
    public NoteListenerActions @NoteListener => new NoteListenerActions(this);

    // Debugger
    private readonly InputActionMap m_Debugger;
    private IDebuggerActions m_DebuggerActionsCallbackInterface;
    private readonly InputAction m_Debugger_ToggleDebugLines;
    public struct DebuggerActions
    {
        private @InputMaster m_Wrapper;
        public DebuggerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleDebugLines => m_Wrapper.m_Debugger_ToggleDebugLines;
        public InputActionMap Get() { return m_Wrapper.m_Debugger; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebuggerActions set) { return set.Get(); }
        public void SetCallbacks(IDebuggerActions instance)
        {
            if (m_Wrapper.m_DebuggerActionsCallbackInterface != null)
            {
                @ToggleDebugLines.started -= m_Wrapper.m_DebuggerActionsCallbackInterface.OnToggleDebugLines;
                @ToggleDebugLines.performed -= m_Wrapper.m_DebuggerActionsCallbackInterface.OnToggleDebugLines;
                @ToggleDebugLines.canceled -= m_Wrapper.m_DebuggerActionsCallbackInterface.OnToggleDebugLines;
            }
            m_Wrapper.m_DebuggerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ToggleDebugLines.started += instance.OnToggleDebugLines;
                @ToggleDebugLines.performed += instance.OnToggleDebugLines;
                @ToggleDebugLines.canceled += instance.OnToggleDebugLines;
            }
        }
    }
    public DebuggerActions @Debugger => new DebuggerActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IPaddleActions
    {
        void OnMoveP1(InputAction.CallbackContext context);
        void OnMoveP2(InputAction.CallbackContext context);
        void OnSprintP1(InputAction.CallbackContext context);
        void OnSprintP2(InputAction.CallbackContext context);
    }
    public interface INoteListenerActions
    {
        void OnMousePos(InputAction.CallbackContext context);
    }
    public interface IDebuggerActions
    {
        void OnToggleDebugLines(InputAction.CallbackContext context);
    }
}
