using System.Collections;
using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;

public class RangedAttackState : AttackState
{
    protected D_RangedAttackState stateData;
    protected Transform attackPosition;
    public RangedAttackState(Entity entity, string animBoolName, Transform attackPosition, D_RangedAttackState stateData) : base(entity, animBoolName, attackPosition.gameObject)
    {
        this.stateData = stateData;
        this.attackPosition = attackPosition; // Store the attack position
    }

    public override void DoChecks()
    {
        base.DoChecks();

        // Check if the player is still within the maximum agro distance
        isPlayerInAgroRange = entity.CheckPlayerInAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        // Reset attack animation flag
        isAnimationFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        // Instantiate the projectile and set its properties
        GameObject projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance);
        }

        Damage damageComponent = projectile.GetComponent<Damage>();
        if (damageComponent != null)
        {
            damageComponent.SetDamage(stateData.projectileDamage);
        }
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        isAnimationFinished = true; // Mark the animation as finished
    }
}