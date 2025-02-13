using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : Entity
{
    public RE_IdleState idleState { get; private set; }
    public RE_MoveState moveState { get; private set; }
    public RE_PlayerDetectedState playerDetectedState { get; private set; }
    public RE_ChargeState chargeState { get; private set; }
    public RE_LookForPlayerState lookForPlayerState { get; private set; }
    public RE_MeleeAttackState meleeAttackState{get;private set;}
    public RE_StunState stunState { get; private set; }
    public RE_DeadState deadState { get; private set; }
    public RE_CooldownState cooldownState { get; private set; }
    public RE_RangedAttackState rangedAttackState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
     [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_CooldownState cooldownStateData;
    [SerializeField]
    private D_RangedAttackState rangedAttackStateData;

    private GameObject meleeAttackCollider;

    [SerializeField]
    private Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();
        meleeAttackCollider = transform.Find("MeleeAttackCollider").gameObject;
        // Debug.Log("meleeAttackCollider: " + meleeAttackCollider);


        moveState = new RE_MoveState(this, "move", moveStateData, this);
        idleState = new RE_IdleState(this, "idle", idleStateData, this);
        playerDetectedState = new RE_PlayerDetectedState(this, "playerDetected", playerDetectedData, this);
        chargeState = new RE_ChargeState(this, "charge", chargeStateData, this); // was charge
        lookForPlayerState = new RE_LookForPlayerState(this, "lookForPlayer", lookForPlayerStateData, this); // was lookForPlayer
        meleeAttackState = new RE_MeleeAttackState(this, "meleeAttack", meleeAttackCollider, meleeAttackStateData, this);
        stunState = new RE_StunState(this, "stun", stunStateData, this);
        deadState = new RE_DeadState(this, "dead", deadStateData, this);
        cooldownState = new RE_CooldownState(this, "cooldown", cooldownStateData, this);
        rangedAttackState = new RE_RangedAttackState(this, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this );
        stats.Stun.OnCurrentValueZero += HandleStunZero;
        stats.Health.OnValueChange += HandleDamageTaken;
    }

    private void HandleStunZero()
    {
        // Debug.Log("HandleStunZero");
        stateMachine.ChangeState(stunState);
        stats.Stun.CurrentValue = stats.Stun.MaxValue;
    }

    private void HandleDamageTaken()
    {
        // Debug.Log("HandleStunZero");
        if(stateMachine.currentState == moveState || stateMachine.currentState == idleState)
        {
            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
    }

    private void OnDisable() {
        stats.Stun.OnCurrentValueZero -= HandleStunZero;
        stats.Health.OnValueChange -= HandleDamageTaken;
    }
}
