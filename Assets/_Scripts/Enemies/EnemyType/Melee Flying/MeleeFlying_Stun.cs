using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFlying_Stun : StunState
{
    private MeleeFlying enemy;

    public MeleeFlying_Stun(Entity entity, string animBoolName, D_StunState stateData, MeleeFlying enemy) : base(entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        enemy.meleeAttackState.DisableHitbox();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.meleeAttackState.DisableHitbox();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isStunTimeOver)
        {
            if (isPlayerInPursuitRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
