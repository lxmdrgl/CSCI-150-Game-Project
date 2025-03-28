﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : EnemyState
{
    protected D_DeadState stateData;

    public DeadState(Entity entity, string animBoolName, D_DeadState stateData) : base(entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        //GameObject.Instantiate(stateData.deathBloodParticle, entity.transform.position, stateData.deathBloodParticle.transform.rotation);
        //GameObject.Instantiate(stateData.deathChunkParticle, entity.transform.position, stateData.deathChunkParticle.transform.rotation);

        //entity.gameObject.SetActive(false);       //already set in CoreComponents->Death.cs
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
