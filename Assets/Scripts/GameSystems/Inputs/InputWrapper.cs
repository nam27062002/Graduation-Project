using System;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class InputWrapper
    {
        public bool IsPressed { get; private set; }
        public bool IsReleased => !IsPressed;

        public readonly InputAction m_inputAction;
        public readonly EPlayerInputType m_inputType;
        private readonly Action<InputAction.CallbackContext> m_inputActionOnperformed;
    }
}