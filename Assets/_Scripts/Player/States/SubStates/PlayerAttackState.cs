using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Game.Weapons;


public class PlayerAttackState : PlayerActionState
{
    private Weapon weapon;
    private WeaponGenerator weaponGenerator;

    protected int inputIndex;

    private bool canInterrupt;

    private bool checkFlip;
    private bool checkInterruptable;
    public bool attackEnabled = true;

    public PlayerAttackState(
        Player player,
        string animBoolName,
        Weapon weapon,
        CombatInputs input
    ) : base(player, animBoolName)
    {
        this.weapon = weapon;

        weaponGenerator = weapon.GetComponent<WeaponGenerator>();

        inputIndex = (int)input;

        weapon.OnUseInput += HandleUseInput;

        weapon.EventHandler.OnEnableInterrupt += HandleEnableInterrupt;
        weapon.EventHandler.OnFinish += HandleFinish;
        weapon.EventHandler.OnFlipSetActive += HandleFlipSetActive;
        weapon.EventHandler.OnInterruptableSetActive += HandleInterruptableSetActive;
    }

    private void HandleFlipSetActive(bool value)
    {
        checkFlip = value;
    }

    private void HandleInterruptableSetActive(bool value)
    {
        checkInterruptable = value;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        var playerInputHandler = player.InputHandler;

        var xInput = playerInputHandler.NormInputX;
        var attackInputs = playerInputHandler.AttackInputs;

        weapon.CurrentInput = attackInputs[inputIndex];

        if (checkFlip)
        {
            Movement.CheckIfShouldFlip(xInput);
        }

        if (checkInterruptable) {
            if (jumpInput || dashInput) {
                HandleUseInput();
                Debug.Log("Interrupt attack: " + string.Join(", ", player.InputHandler.AttackInputs));
                isActionDone = true;
            }
        }

        if (!canInterrupt)
            return;

        if (xInput != 0 )
        {
            isActionDone = true;
        }

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        for (int i = 0; i < count; i++) {
            if (playerInputHandler.AttackInputs[i]) {
                isActionDone = true;
                break;
            }
        }
    }

    private void HandleWeaponGenerating()
    {
        stateMachine.ChangeState(player.IdleState);
    }

    public override void Enter()
    {
        base.Enter();

        weaponGenerator.OnWeaponGenerating += HandleWeaponGenerating;
        
        checkFlip = true;
        canInterrupt = false;
        attackEnabled = false;

        weapon.Enter();
        if (inputIndex == (int)CombatInputs.primarySkillPress) {
            player.primarySkillTimeNotifier.Disable();
        } 
        else if (inputIndex == (int)CombatInputs.secondarySkillPress) {
            player.secondarySkillTimeNotifier.Disable();
        } 
        else if (inputIndex == (int)CombatInputs.dashAttack) {
            Debug.Log("Dash attack enabled: " + attackEnabled);
        } 
    }


    public override void Exit()
    {
        base.Exit();

        weaponGenerator.OnWeaponGenerating -= HandleWeaponGenerating;
        if (inputIndex == (int)CombatInputs.primarySkillPress) {
            player.primarySkillTimeNotifier.Init(weapon.Data.AttackCooldown);
            // Debug.Log("Start primary skill cooldown: " + weapon.Data.AttackCooldown);
        } 
        else if (inputIndex == (int)CombatInputs.secondarySkillPress) {
            player.secondarySkillTimeNotifier.Init(weapon.Data.AttackCooldown);
        }
        
        weapon.Exit();
        
    }

    public bool CanAttack() 
    {
        if (!weapon.CanEnterAttack) {
            HandleUseInput();
        }
        return weapon.CanEnterAttack;
    }

    public bool CanAttackCooldown() 
    {
        bool canAttack = weapon.CanEnterAttack && attackEnabled;
        if (!canAttack)
        {
            HandleUseInput();
        }
        return canAttack;
    }

    public bool CanDashAttackCooldown(CombatInputs input, CombatInputs output) 
    {
        bool canAttack = weapon.CanEnterAttack && attackEnabled;
        bool hasInput = false;
        if (canAttack)
        {
            hasInput = player.InputHandler.SimulateAttackInput(input, output);
        }
        // Debug.Log(string.Join(", ", player.InputHandler.AttackInputs));
        return canAttack && hasInput;
    }

    public void ResetAttackCooldown() => attackEnabled = true;
    public void DashAttackCooldownDisable() {
        attackEnabled = false;
        Debug.Log("Reset dash attack cooldown: " + attackEnabled);   
    }

    public void DashAttackCooldownEnable() {
        attackEnabled = true;
        Debug.Log("Reset dash attack cooldown: " + attackEnabled);   
    }

    private void HandleEnableInterrupt() => canInterrupt = true;

    private void HandleUseInput() => player.InputHandler.UseAttackInput(inputIndex);

    private void HandleFinish()
    {
        AnimationFinishTrigger();
        isActionDone = true;
    }
}
