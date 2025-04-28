using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_MeleeDashState : EnemyDashState
{
    private Enemy5 enemy;


    public E5_MeleeDashState(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData, Enemy5 enemy) : base(entity, animBoolName, meleeAttackCollider, stateData, enemy)
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
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    
     public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override bool TriggerAttack()
    {
        base.TriggerAttack();
        return false;
    }
}