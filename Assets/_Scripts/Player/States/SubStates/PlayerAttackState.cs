using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Weapons;


public class PlayerAttackState : PlayerActionState
{
    private Weapon weapon;
    private WeaponGenerator weaponGenerator;

    private int inputIndex;

    private bool canInterrupt;

    private bool checkFlip;
    private bool checkInterruptable;
    protected bool attackEnabled = true;

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
                isActionDone = true;
            }
        }

        if (!canInterrupt)
            return;

        if (xInput != 0 || attackInputs[0] || attackInputs[1] || attackInputs[2] || attackInputs[3])
        {
            isActionDone = true;
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
    
    /* => weapon.CanEnterAttack;/*  && attackEnabled */
    // public bool CanAttackCooldown() => weapon.CanEnterAttack && attackEnabled; */

    public bool CanAttackCooldown() => weapon.CanEnterAttack && attackEnabled;
    

    public void ResetAttackCooldown() => attackEnabled = true;

    private void HandleEnableInterrupt() => canInterrupt = true;

    private void HandleUseInput() => player.InputHandler.UseAttackInput(inputIndex);

    private void HandleFinish()
    {
        AnimationFinishTrigger();
        isActionDone = true;
    }
}
