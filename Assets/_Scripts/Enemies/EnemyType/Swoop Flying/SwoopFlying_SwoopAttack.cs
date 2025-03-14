using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoopFlying_SwoopAttack : MeleeAttackState
{
    private SwoopFlying enemy;
	protected bool isAttackOffCooldown;
    protected bool triggeredAttack;
    private Vector3 startPos;
    private Vector3 swoopTarget;
    private Vector3 returnPos;
    private float swoopProgress;
    private float swoopDuration = 1f; // How long the swoop lasts
    private float swoopHeight = 3f; // How high the enemy swoops

    public SwoopFlying_SwoopAttack(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData, SwoopFlying enemy) : base(entity, animBoolName, meleeAttackCollider, stateData)
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
        triggeredAttack = false;
        swoopProgress = 0f;

        // Store starting position
        startPos = enemy.transform.position;

        // Set the target position 
        swoopTarget = enemy.targetPlayer.position;

        returnPos = startPos + new Vector3(10f * (Movement?.FacingDirection<=0 ? -1 : 1), 0, 0);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!triggeredAttack) {
            //TriggerAttack();
            triggeredAttack = TriggerAttack();
        }

        if (swoopProgress < swoopDuration)
        {
            swoopProgress += Time.deltaTime / swoopDuration;

            // Lerp movement towards the target while applying an arch
            Vector3 horizontalMove = Vector3.Lerp(startPos, returnPos, swoopProgress);
            float arcOffset = Mathf.Sin(swoopProgress * Mathf.PI) * swoopHeight;

            enemy.transform.position = new Vector3(horizontalMove.x, horizontalMove.y - arcOffset, horizontalMove.z);
        }
        else
        {
            // When the swoop is complete, transition to chargeState
            stateMachine.ChangeState(enemy.chargeState);
        }

        // Needs a time
        // if (Time.time >= startTime + swoopTime)
        // {
        //     //isAttackOver = true;
        //     FinishAttack();
        //     //stateMachine.ChangeState(enemy.chargeState);
        // }
        
        // if (isAnimationFinished)
        // {
        //     stateMachine.ChangeState(enemy.chargeState);
        // }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override bool TriggerAttack()
    {
        return base.TriggerAttack();
        //return false;
    }
}
