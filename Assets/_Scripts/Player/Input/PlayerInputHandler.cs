using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<bool> OnInteractInputChanged; 

    public static PlayerInput playerInput;
    
    public Vector2 RawMovementInput { get; private set; }

    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool DownInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool MenuOpenInput { get; private set; }
    public bool UIMenuCloseInput { get; private set; }
    public bool UpgradeOpenInput { get; private set; }

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

    public void OnDownInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Input Down start");
            DownInput = true;
        }
        
        if (context.canceled)
        {
            Debug.Log("Input Down end");
            DownInput = false;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Input Jump start");
            JumpInput = true;
        }
        
        if (context.canceled)
        {
            Debug.Log("Input Jump start");
            JumpInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            // Debug.Log("Start dash");
        }
        
        if (context.canceled)
        {
            DashInput = false;
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
        if (context.performed)
        {
            if (context.interaction is UnityEngine.InputSystem.Interactions.HoldInteraction)
            {
                // Hold attack
                AttackInputs[(int)CombatInputs.primaryAttackHold] = true;
                Debug.Log("Primary Attack Hold Input: " + context.duration);
            }
            else if (context.interaction is UnityEngine.InputSystem.Interactions.PressInteraction)
            {
                // Press attack (quick tap)
                AttackInputs[(int)CombatInputs.primaryAttackPress] = true;
                Debug.Log("Primary Attack Press Input");
            }
        }

        if (context.canceled)
        {
            // Reset both inputs when released
            AttackInputs[(int)CombatInputs.primaryAttackPress] = false;
            AttackInputs[(int)CombatInputs.primaryAttackHold] = false;
            Debug.Log("Primary Attack End Input");
        }
    }


    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.secondaryAttackPress] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondaryAttackPress] = false;
        }
    }

    public void OnPrimarySkillInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.primarySkillPress] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.primarySkillPress] = false;
        }
    }

    public void OnSecondarySkillInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.secondarySkillPress] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondarySkillPress] = false;
        }
    }



    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;

    public void UseAttackInput(int i) => AttackInputs[i] = false;

    public void OnMenuOpenInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MenuOpenInput = true;
            
        }
        
        if (context.canceled)
        {
            MenuOpenInput = false;
        }
    }

    public void UseMenuOpenInput() => MenuOpenInput = false;

     public void OnUIMenuCloseInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UIMenuCloseInput = true;
            
        }
        
        if (context.canceled)
        {
            UIMenuCloseInput = false;
        }
    }

    public void UseUIMenuCloseInput() => UIMenuCloseInput = false;

    public void OnUpgradeOpenInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UpgradeOpenInput = true;
            Debug.Log("Upgrade Open Input");
        }
        
        if (context.canceled)
        {
            UpgradeOpenInput = false;
        }
    }

    public void UseUpgradeOpenInput() => UpgradeOpenInput = false;
}

public enum CombatInputs{
    primaryAttackPress,
    secondaryAttackPress,
    primarySkillPress,
    secondarySkillPress,
    primaryAttackHold
}

