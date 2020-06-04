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
        // NoteListener
        m_NoteListener = asset.FindActionMap("NoteListener", throwIfNotFound: true);
        m_NoteListener_MousePos = m_NoteListener.FindAction("MousePos", throwIfNotFound: true);
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
    public struct PaddleActions
    {
        private @InputMaster m_Wrapper;
        public PaddleActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveP1 => m_Wrapper.m_Paddle_MoveP1;
        public InputAction @MoveP2 => m_Wrapper.m_Paddle_MoveP2;
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
    }
    public interface INoteListenerActions
    {
        void OnMousePos(InputAction.CallbackContext context);
    }
}
