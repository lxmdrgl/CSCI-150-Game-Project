using UnityEngine;

public class EE_ExplosiveAttackState : ExplosiveAttackState
{
    private ExplosiveEnemy enemy;

    public EE_ExplosiveAttackState(Entity entity, string animBoolName, Transform attackPosition, D_ExplosiveAttackState stateData, ExplosiveEnemy enemy) 
        : base(entity, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        isAnimationFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Face the player
        Vector2 direction = (enemy.player.position - entity.transform.position).normalized;
        if ((direction.x > 0 && Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
        {
            Movement?.Flip();
        }

        // Throw a single explosive when the animation finishes
        if (isAnimationFinished)
        {
            // Calculate direction to the player
            Vector2 playerDir = (enemy.player.position - attackPosition.position).normalized;
            float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // Spawn the explosive projectile
            GameObject projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, rotation);
            ExplosiveProjectile projectileScript = projectile.GetComponent<ExplosiveProjectile>();

            if (projectileScript != null)
            {
                // Launch the projectile toward the player
                projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance);
            }
            else
            {
                Debug.LogError("ExplosiveProjectile component not found on instantiated projectile.");
            }

            // Transition to the next state
            if (isPlayerInPursuitRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        isAnimationFinished = true;
    }
}