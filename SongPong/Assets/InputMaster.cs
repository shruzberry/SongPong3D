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
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""97324925-b0e1-4fd1-a005-662f6c8e0824"",
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
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""393ad0c7-ce59-4631-a862-a5623c01b2c5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d080a7da-60e3-41ee-a84f-3a474cab34fd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c190f410-b3c7-443a-a29b-ab2fa8c18bcb"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d6fd1796-2560-4576-8392-a23ca5bc01f9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Move"",
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
                    ""name"": ""Mouse Pos"",
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
                    ""action"": ""Mouse Pos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard Left"",
            ""bindingGroup"": ""Keyboard Left"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Paddle
        m_Paddle = asset.FindActionMap("Paddle", throwIfNotFound: true);
        m_Paddle_Move = m_Paddle.FindAction("Move", throwIfNotFound: true);
        // NoteListener
        m_NoteListener = asset.FindActionMap("NoteListener", throwIfNotFound: true);
        m_NoteListener_MousePos = m_NoteListener.FindAction("Mouse Pos", throwIfNotFound: true);
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
    private readonly InputAction m_Paddle_Move;
    public struct PaddleActions
    {
        private @InputMaster m_Wrapper;
        public PaddleActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Paddle_Move;
        public InputActionMap Get() { return m_Wrapper.m_Paddle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PaddleActions set) { return set.Get(); }
        public void SetCallbacks(IPaddleActions instance)
        {
            if (m_Wrapper.m_PaddleActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PaddleActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PaddleActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PaddleActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_PaddleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
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
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    private int m_KeyboardLeftSchemeIndex = -1;
    public InputControlScheme KeyboardLeftScheme
    {
        get
        {
            if (m_KeyboardLeftSchemeIndex == -1) m_KeyboardLeftSchemeIndex = asset.FindControlSchemeIndex("Keyboard Left");
            return asset.controlSchemes[m_KeyboardLeftSchemeIndex];
        }
    }
    public interface IPaddleActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
    public interface INoteListenerActions
    {
        void OnMousePos(InputAction.CallbackContext context);
    }
}
