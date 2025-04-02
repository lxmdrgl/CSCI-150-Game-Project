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
        
        Vector2 direction = (enemy.targetPlayer.position - enemy.transform.position).normalized;
        Vector3 target = enemy.targetPlayer.position;

        if ((direction.x > 0 && Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
        {
            Movement?.Flip();
        }

        if (isAnimationFinished)
        {
            // Instantiate the projectile and set its properties
            GameObject projectile1 = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            ProjectileEnemy projectileScript = projectile1.GetComponent<ProjectileEnemy>();
            // GameObject projectile2 = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            // ProjectileEnemy projectileScript2 = projectile2.GetComponent<ProjectileEnemy>();
            // GameObject projectile3 = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            // ProjectileEnemy projectileScript3 = projectile3.GetComponent<ProjectileEnemy>();

            if (projectileScript != null)
            {
                Vector3 projectilePosition1 = target;
                // Vector3 projectilePosition2 = new Vector3(target.x - 2.0f, target.y, target.z);
                // Vector3 projectilePosition3 = new Vector3(target.x + 2.0f, target.y, target.z);
                projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, projectilePosition1, "radialLobbing", enemy.transform.rotation.y, stateData.gravityScale);
                // projectileScript2.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, projectilePosition2, "radialLobbing", enemy.transform.rotation.y, stateData.gravityScale);
                // projectileScript3.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, projectilePosition3, "radialLobbing", enemy.transform.rotation.y, stateData.gravityScale);
            }
            /* else
            {
                ExplosiveProjectileEnemy explosiveProjectileScript = projectile1.GetComponent<ExplosiveProjectileEnemy>();
                if (explosiveProjectileScript != null)
                {
                    Debug.Log("✅ Found ExplosiveProjectile script, firing projectile...");
                    explosiveProjectileScript.FireProjectile(20f, stateData.projectileTravelDistance);
                }
            } */


            DamageEnemy damageComponent1 = projectile1.GetComponentInChildren<DamageEnemy>();
            // DamageEnemy damageComponent2 = projectile2.GetComponentInChildren<DamageEnemy>();
            // DamageEnemy damageComponent3 = projectile3.GetComponentInChildren<DamageEnemy>();
            if (damageComponent1 != null)
            {
                damageComponent1.SetDamage(stateData.projectileDamage, stateData.knockbackAngle, stateData.knockbackStrength);
                // damageComponent2.SetDamage(stateData.projectileDamage, stateData.knockbackAngle, stateData.knockbackStrength);
                // damageComponent3.SetDamage(stateData.projectileDamage, stateData.knockbackAngle, stateData.knockbackStrength);
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