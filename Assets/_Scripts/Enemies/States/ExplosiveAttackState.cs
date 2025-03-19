using System.Collections;
using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;
using Game.CoreSystem;

public class ExplosiveAttackState : AttackState
{
    protected D_ExplosiveAttackState stateData;
    protected Transform attackPosition;

    public ExplosiveAttackState(Entity entity, string animBoolName, Transform attackPosition, D_ExplosiveAttackState stateData) 
        : base(entity, animBoolName, attackPosition.gameObject)
    {
        this.stateData = stateData;
        this.attackPosition = attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInAgroRange = entity.CheckPlayerInMaxAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isAnimationFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (isAnimationFinished)
        {
            if (isPlayerInAgroRange)
            {
                TriggerAttack();
            }
        }
    }

    public override bool TriggerAttack() 
    {
        base.TriggerAttack();

        Debug.Log("⚠️ Spawning explosive projectile...");

        // Instantiate the explosive projectile at the attack position
        GameObject projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        ExplosiveProjectileEnemy projectileScript = projectile.GetComponent<ExplosiveProjectileEnemy>();

        if (projectileScript != null)
        {
            Debug.Log("✅ Found ExplosiveProjectile script, firing projectile...");
            projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance);
            return true; // Return true after successful attack
        }
        else
        {
            Debug.LogError("❌ No ExplosiveProjectile script found on spawned object!");
            return false; // Return false if attack failed
        }
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        isAnimationFinished = true;
    }
}