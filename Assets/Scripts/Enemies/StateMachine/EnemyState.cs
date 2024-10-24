using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Entity entity;
    protected Core core;    

    public float startTime { get; protected set; }

    protected string animBoolName;

    public EnemyState(Entity etity,  string animBoolName)
    {
        this.entity = etity;
        this.animBoolName = animBoolName;
        stateMachine = entity.stateMachine;
        core = entity.Core;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        entity.anim.SetBool(animBoolName, true);
        DoChecks();
    }

    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }
}
