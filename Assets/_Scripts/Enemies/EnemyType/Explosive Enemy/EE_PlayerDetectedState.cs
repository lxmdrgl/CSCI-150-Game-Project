using UnityEngine;

public class EE_PlayerDetectedState : PlayerDetectedState
{
    private ExplosiveEnemy enemy;

    public EE_PlayerDetectedState(Entity entity, string animBoolName, D_PlayerDetected stateData, ExplosiveEnemy enemy) 
        : base(entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isDetectedTimeOver)
        {
            if (isPlayerInPursuitRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else if (!isDetectingLedge)
            {
                Movement?.Flip();
                stateMachine.ChangeState(enemy.moveState);
            }
            else
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}