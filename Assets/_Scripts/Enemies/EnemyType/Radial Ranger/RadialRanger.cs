using Unity.VisualScripting;
using UnityEngine;

public class RadialRanger : Entity
{
    public RR_IdleState idleState { get; private set; }
    public RR_MoveState moveState { get; private set; }
    public RR_PlayerDetectedState playerDetectedState { get; private set; }
    public RR_ChargeState chargeState { get; private set; }
    public RR_LookForPlayerState lookForPlayerState { get; private set; }
    public RR_MeleeAttackState meleeAttackState{get;private set;}
    public RR_StunState stunState { get; private set; }
    public RR_DeadState deadState { get; private set; }
    public RR_CooldownState cooldownState { get; private set; }
    public RR_RangedAttackState rangedAttackState { get; private set; }
    public RR_UpwardRangedAttack radialRangedAttackState { get; private set; }

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
    [SerializeField]
    private Transform radialRangedAttackPosition;
    public override void Awake()
    {
        base.Awake();
        meleeAttackCollider = transform.Find("MeleeAttackCollider").gameObject;
        // Debug.Log("meleeAttackCollider: " + meleeAttackCollider);


        moveState = new RR_MoveState(this, "move", moveStateData, this);
        idleState = new RR_IdleState(this, "idle", idleStateData, this);
        playerDetectedState = new RR_PlayerDetectedState(this, "playerDetected", playerDetectedData, this);
        chargeState = new RR_ChargeState(this, "charge", chargeStateData, this); // was charge
        lookForPlayerState = new RR_LookForPlayerState(this, "lookForPlayer", lookForPlayerStateData, this); // was lookForPlayer
        meleeAttackState = new RR_MeleeAttackState(this, "meleeAttack", meleeAttackCollider, meleeAttackStateData, this);
        stunState = new RR_StunState(this, "stun", stunStateData, this);
        deadState = new RR_DeadState(this, "dead", deadStateData, this);
        cooldownState = new RR_CooldownState(this, "cooldown", cooldownStateData, this);
        rangedAttackState = new RR_RangedAttackState(this, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this );
        radialRangedAttackState = new RR_UpwardRangedAttack(this, "radialRangedAttack", radialRangedAttackPosition, rangedAttackStateData, this );
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
