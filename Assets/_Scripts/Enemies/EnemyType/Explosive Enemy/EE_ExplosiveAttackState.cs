using UnityEngine;
using Game.Projectiles;

public class EE_ExplosiveAttackState : ExplosiveAttackState
{
    private ExplosiveEnemy enemy;

    public EE_ExplosiveAttackState(Entity entity, string animBoolName, Transform attackPosition, D_ExplosiveAttackState stateData, ExplosiveEnemy enemy) 
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            //Vector2 direction = (enemy.targetPlayer.position - enemy.transform.position).normalized;
            GameObject projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
            ExplosiveProjectileEnemy projectileScript = projectile.GetComponent<ExplosiveProjectileEnemy>();
            if (projectileScript != null)
            {
                projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance,enemy.targetPlayer.position,"radialNoGravity", enemy.transform.rotation.y, stateData.gravityScale);
            }

            DamageEnemy damageComponent = projectile.GetComponentInChildren<DamageEnemy>();
            if (damageComponent != null)
            {
                damageComponent.SetDamage(stateData.projectileDamage, stateData.knockbackAngle, stateData.knockbackStrength);
            }

            if (isPlayerInAgroRange) // Player is in agro range
            {
                stateMachine.ChangeState(enemy.explosiveAttackState); // Transition to PlayerDetectedState
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
        isAnimationFinished = true;
    }
}