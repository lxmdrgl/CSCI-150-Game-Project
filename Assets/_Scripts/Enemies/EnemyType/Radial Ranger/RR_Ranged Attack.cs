using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;


public class RR_RangedAttackState : RangedAttackState
{
    private RadialRanger enemy;

    public RR_RangedAttackState(Entity entity, string animBoolName, Transform attackPosition, D_RangedAttackState stateData, RadialRanger enemy) 
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

        Vector2 direction = (enemy.player.position - enemy.transform.position).normalized;

        if ((direction.x > 0 &&  Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
        {
            Movement?.Flip();
        }

        if (isAnimationFinished)
        {
            // Instantiate the projectile and set its properties
            GameObject projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                // projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, enemy.player.position, "radialWithGravity", enemy.transform.rotation.y, stateData.gravityScale);

                Vector3 projectile2 = new Vector3(enemy.player.position.x - 2.0f, enemy.player.position.y, enemy.player.position.z);
                Vector3 projectile3 = new Vector3(enemy.player.position.x + 2.0f, enemy.player.position.y, enemy.player.position.z);
                projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, enemy.player.position, "radialLobbing", enemy.transform.rotation.y, stateData.gravityScale);
                // projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, projectile2, "radialLobbing", enemy.transform.rotation.y, stateData.gravityScale);
                // projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, projectile3, "radialLobbing", enemy.transform.rotation.y, stateData.gravityScale);
            }

            Damage damageComponent = projectile.GetComponentInChildren<Damage>();
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