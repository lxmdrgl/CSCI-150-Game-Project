using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Game.CoreSystem;
using Game.Utilities;

public class PlayerState
{
    protected Core core; 
    protected Player player;
    protected PlayerStateMachine stateMachine; 
    protected PlayerData playerData;

    
    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    private string animBoolName;

    public PlayerState(Player player, string animBoolName)
    {
        this.player = player;
        this.animBoolName = animBoolName;
        stateMachine = player.StateMachine;
        playerData = player.playerData;
        core = player.Core; 
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true; 
    }

    public virtual void LogicUpdate()
    {
        player.dashTimeNotifier.Tick();
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
