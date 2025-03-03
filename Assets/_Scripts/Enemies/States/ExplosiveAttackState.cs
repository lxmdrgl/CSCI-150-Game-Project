/*using System.Collections;
using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;
using Game.CoreSystem;

public class ExplosiveAttackState : RangedAttackState
{
    protected D_ExplosiveAttackState explosiveStateData;

    public ExplosiveAttackState(Entity entity, string animBoolName, Transform attackPosition, D_ExplosiveAttackState stateData) 
        : base(entity, animBoolName, attackPosition, stateData)
    {
        this.explosiveStateData = stateData;
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Debug.Log("⚠️ Spawning explosive projectile...");

        // Instantiate the explosive projectile at the attack position
        GameObject projectile = GameObject.Instantiate(explosiveStateData.projectile, attackPosition.position, attackPosition.rotation);
        ExplosiveProjectile projectileScript = projectile.GetComponent<ExplosiveProjectile>();

        if (projectileScript != null)
        {
            Debug.Log("✅ Found ExplosiveProjectile script, firing projectile...");
            projectileScript.FireProjectile(explosiveStateData.projectileSpeed, explosiveStateData.projectileTravelDistance);
        }
        else
        {
            Debug.LogError("❌ No ExplosiveProjectile script found on spawned object!");
        }
    }
}
*/