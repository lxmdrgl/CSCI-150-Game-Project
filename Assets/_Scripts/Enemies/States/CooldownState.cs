using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Game.CoreSystem;
public class CooldownState : EnemyState
{
    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    
    private Movement movement;
	private CollisionSenses collisionSenses;
    protected D_CooldownState stateData;

    protected bool flipAfterIdle;
    protected bool isCooldownTimeOver;
    protected bool isPlayerInAgroRange;
    protected bool isPlayerInPursuitRange;
    protected float cooldownTime;

    public CooldownState(Entity entity, string animBoolName, D_CooldownState stateData) : base(entity, animBoolName)
    {
        this.stateData = stateData;
        cooldownTime = stateData.attackCooldown;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInAgroRange = entity.CheckPlayerInAgroRange();
        isPlayerInPursuitRange = entity.CheckPlayerInPursuitRange();
    }

    public override void Enter()
    {
        base.Enter();

        isCooldownTimeOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + cooldownTime)
        {
            isCooldownTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
