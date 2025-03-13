using UnityEngine;

public class ExplosiveAttackState : AttackState
{
    protected D_ExplosiveAttackState stateData; // Data for this state (e.g., projectile prefab)
    protected Transform attackPosition;         // Where the projectile spawns
    //private bool isAnimationFinished;           // Tracks when the attack animation ends

    // State data for transitions
    protected D_CooldownState cooldownData;
    protected D_IdleState idleData;

    // Constructor: Pass entity, animation name, attack position, and state data
    public ExplosiveAttackState(Entity entity, string animBoolName, Transform attackPosition, 
        D_ExplosiveAttackState stateData, D_CooldownState cooldownData, D_IdleState idleData) 
        : base(entity, animBoolName, attackPosition.gameObject)
    {
        this.stateData = stateData;
        this.attackPosition = attackPosition;
        this.cooldownData = cooldownData;
        this.idleData = idleData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInAgroRange = entity.CheckPlayerInAgroRange(); // Check if player is in range
    }

    public override void Enter()
    {
        base.Enter();
        isAnimationFinished = false; // Reset animation flag
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Transition to cooldown or idle after animation finishes
        if (isAnimationFinished)
        {
            if (isPlayerInAgroRange && cooldownData != null)
            {
                entity.stateMachine.ChangeState(new CooldownState(entity, "cooldown", cooldownData));
            }
            else
            {
                entity.stateMachine.ChangeState(new IdleState(entity, "idle", idleData));
            }
        }
    }

    public override bool TriggerAttack()
    {
        base.TriggerAttack();

        // Spawn the explosive projectile
        GameObject projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        ExplosiveProjectile projectileScript = projectile.GetComponent<ExplosiveProjectile>();

        if (projectileScript != null)
        {
            projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance);
            return true; // Successfully spawned
        }
        else
        {
            Debug.LogError("No ExplosiveProjectile script found on spawned object!");
            return false;
        }
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        isAnimationFinished = true; // Mark animation as complete
    }
}