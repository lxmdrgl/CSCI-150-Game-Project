using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }

    private Movement movement;
    private CollisionSenses collisionSenses;

    private bool jumpInput;
    private bool isGrounded;
    private bool isTouchingWall;

    public PlayerGroundedState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
        }
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;

        if (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttack] /* && player.PrimaryAttackState.CanTransitionToAttackState() */)
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
            Debug.Log("Attack from ground");
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttack] && player.SecondaryAttackState.CanTransitionToAttackState())
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        } else if (!isGrounded)
        {
            player.AirState.StartCoyoteTime();
            stateMachine.ChangeState(player.AirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
