using UnityEngine;

public class EE_ChargeState : ChargeState
{
    private ExplosiveEnemy enemy;

    public EE_ChargeState(Entity entity, string animBoolName, D_ChargeState stateData, ExplosiveEnemy enemy)
        : base(entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isPlayerInPursuitRange && isPlayerInAgroRange)
        {
            stateMachine.ChangeState(enemy.explosiveAttackState); // Transition to explosive attack
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState); // Transition to look for player
        }
    }
    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}