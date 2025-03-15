using System.Collections;
using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;
using Game.CoreSystem;

public class RangedAttackState : AttackState
{
    public Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private Movement movement;

    protected D_RangedAttackState stateData;
    protected Transform attackPosition;
    public RangedAttackState(Entity entity, string animBoolName, Transform attackPosition, D_RangedAttackState stateData) : base(entity, animBoolName, attackPosition.gameObject)
    {
        this.stateData = stateData;
        this.attackPosition = attackPosition; // Store the attack position
    }

    public override void DoChecks()
    {
        base.DoChecks();

        // Check if the player is still within the maximum agro distance
        isPlayerInAgroRange = entity.CheckPlayerInAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        // Reset attack animation flag
        isAnimationFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }

    public override bool TriggerAttack()
    {
        base.TriggerAttack();

        return false;
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        isAnimationFinished = true; // Mark the animation as finished
    }
}