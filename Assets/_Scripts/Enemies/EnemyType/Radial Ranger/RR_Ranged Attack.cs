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
            GameObject projectile1 = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            Projectile projectileScript = projectile1.GetComponent<Projectile>();
            GameObject projectile2 = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            Projectile projectileScript2 = projectile2.GetComponent<Projectile>();
            GameObject projectile3 = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            Projectile projectileScript3 = projectile3.GetComponent<Projectile>();
            
            if (projectileScript != null)
            {
                // projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, enemy.player.position, "radialWithGravity", enemy.transform.rotation.y, stateData.gravityScale);
                Vector3 projectilePosition1 = enemy.player.position;
                Vector3 projectilePosition2 = new Vector3(enemy.player.position.x - 2.0f, enemy.player.position.y, enemy.player.position.z);
                Vector3 projectilePosition3 = new Vector3(enemy.player.position.x + 2.0f, enemy.player.position.y, enemy.player.position.z);
                projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, projectilePosition1, "radialLobbing", enemy.transform.rotation.y, stateData.gravityScale);
                projectileScript2.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, projectilePosition2, "radialLobbing", enemy.transform.rotation.y, stateData.gravityScale);
                projectileScript3.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, projectilePosition3, "radialLobbing", enemy.transform.rotation.y, stateData.gravityScale);
            }

            Damage damageComponent1 = projectile1.GetComponentInChildren<Damage>();
            Damage damageComponent2 = projectile2.GetComponentInChildren<Damage>();
            Damage damageComponent3 = projectile3.GetComponentInChildren<Damage>();
            if (damageComponent1 != null)
            {
                damageComponent1.SetDamage(stateData.projectileDamage, stateData.knockbackAngle, stateData.knockbackStrength);
                damageComponent2.SetDamage(stateData.projectileDamage, stateData.knockbackAngle, stateData.knockbackStrength);
                damageComponent3.SetDamage(stateData.projectileDamage, stateData.knockbackAngle, stateData.knockbackStrength);
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