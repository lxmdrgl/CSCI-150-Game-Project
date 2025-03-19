using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;

public class RR_UpwardRangedAttack : RangedAttackState
{
    private RadialRanger enemy;

    public RR_UpwardRangedAttack(Entity entity, string animBoolName, Transform attackPosition, D_RangedAttackState stateData, RadialRanger enemy) 
        : base(entity, animBoolName, attackPosition, stateData)
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
        isAnimationFinished = false;
    }

    public override void Exit()
    {
        base.Exit();
        // Cleanup or reset anything when exiting this state
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Vector2 direction = (enemy.targetPlayer.position - enemy.transform.position).normalized;

        if ((direction.x > 0 &&  Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
        {
            Movement?.Flip();
        }

        if (isAnimationFinished)
        {
            // Instantiate the projectile and set its properties
            GameObject projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            ProjectileEnemy projectileScript = projectile.GetComponent<ProjectileEnemy>();
            if (projectileScript != null)
            {
                projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, enemy.targetPlayer.position, "radialNoGravity", enemy.transform.rotation.y, stateData.gravityScale);
            }

            DamageEnemy damageComponent = projectile.GetComponentInChildren<DamageEnemy>();
            if (damageComponent != null)
            {
                damageComponent.SetDamage(stateData.projectileDamage, stateData.knockbackAngle, stateData.knockbackStrength);
            }

            if (isPlayerInPursuitRange) // Player is in agro range
            {
                stateMachine.ChangeState(enemy.chargeState); // Transition to PlayerDetectedState
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState); // Transition to LookForPlayerState
            }
        }
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

    public override void FinishAttack()
    {
        base.FinishAttack();
        isAnimationFinished = true; // Mark the animation as finished
    }
}