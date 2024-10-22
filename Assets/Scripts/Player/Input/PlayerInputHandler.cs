using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<bool> OnInteractInputChanged; 

    private PlayerInput playerInput;
    
    public Vector2 RawMovementInput { get; private set; }

    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }

    public bool[] AttackInputs { get; private set; }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>(); 
        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];
    }

    private void Update()
    {

    }

    public void OnMoveInput(InputAction.CallbackContext context) 
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
        }

        if (context.canceled)
        {
            JumpInput = false;
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInteractInputChanged?.Invoke(true);
            return;
        }

        if (context.canceled)
        {
            OnInteractInputChanged?.Invoke(false);
        }
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.primaryAttack] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.primaryAttack] = false;
        }
    }



    public void UseJumpInput() => JumpInput = false;

    public void UseAttackInput(int i) => AttackInputs[i] = false;
}

public enum CombatInputs{
    primaryAttack,
    secondaryAttack,
    primarySkill,
    secondarySkill
}
