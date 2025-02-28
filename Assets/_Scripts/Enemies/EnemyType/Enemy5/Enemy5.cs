using Unity.VisualScripting;
using UnityEngine;

public class Enemy5 : Entity
{
    public E5_IdleState idleState { get; private set; }
    public E5_MoveState moveState { get; private set; }
    public E5_PlayerDetectedState playerDetectedState { get; private set; }
    public E5_ChargeState chargeState { get; private set; }
    public E5_LookForPlayerState lookForPlayerState { get; private set; }
    public E5_MeleeAttackState meleeAttackState{get;private set;}
    public E5_StunState stunState { get; private set; }
    public E5_DeadState deadState { get; private set; }
    public E5_CooldownState cooldownState { get; private set; }

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


    private GameObject meleeAttackCollider;


    public override void Awake()
    {
        base.Awake();
        meleeAttackCollider = transform.Find("MeleeAttackCollider").gameObject;
        // Debug.Log("meleeAttackCollider: " + meleeAttackCollider);


        moveState = new E5_MoveState(this, "move", moveStateData, this);
        idleState = new E5_IdleState(this, "idle", idleStateData, this);
        playerDetectedState = new E5_PlayerDetectedState(this, "playerDetected", playerDetectedData, this);
        chargeState = new E5_ChargeState(this, "charge", chargeStateData, this); // was charge
        lookForPlayerState = new E5_LookForPlayerState(this, "lookForPlayer", lookForPlayerStateData, this); // was lookForPlayer
        meleeAttackState = new E5_MeleeAttackState(this, "meleeAttack", meleeAttackCollider, meleeAttackStateData, this);
        stunState = new E5_StunState(this, "stun", stunStateData, this);
        deadState = new E5_DeadState(this, "dead", deadStateData, this);
        cooldownState = new E5_CooldownState(this, "cooldown", cooldownStateData, this);

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
