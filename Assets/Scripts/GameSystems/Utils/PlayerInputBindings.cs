using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public enum EPlayerInputType
    {
        Movement = 0,
        Jump = 1,
        Attack = 2,
    }
    
    public enum EPlayerInputControlSchemes
    {
        Pad,
        Keyboard,
    }
    
    public class PlayerInputBindings
    {
        public PadInput PlayerInput { get; } = new PadInput();
        public Dictionary<EPlayerInputType, InputAction> PlayerInputTypeMap { get; } 
        public Dictionary<InputAction, EPlayerInputType> PlayerInputTypeMapReverse { get; }

        public PlayerInputBindings()
        {
            PlayerInputTypeMap = new Dictionary<EPlayerInputType, InputAction>();
            PlayerInputTypeMapReverse = new Dictionary<InputAction, EPlayerInputType>();
            
            // Movement
            PlayerInputTypeMap.Add(EPlayerInputType.Movement, PlayerInput.Gameplay.Movement);
            PlayerInputTypeMapReverse.Add(PlayerInput.Gameplay.Movement, EPlayerInputType.Movement);
            // Jump
            PlayerInputTypeMap.Add(EPlayerInputType.Jump, PlayerInput.Gameplay.Jump);
            PlayerInputTypeMapReverse.Add(PlayerInput.Gameplay.Jump, EPlayerInputType.Jump);
            // Attack
            PlayerInputTypeMap.Add(EPlayerInputType.Attack, PlayerInput.Gameplay.Attack);
            PlayerInputTypeMapReverse.Add(PlayerInput.Gameplay.Attack, EPlayerInputType.Attack);
        }
    }
}