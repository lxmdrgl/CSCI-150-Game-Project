using Unity.VisualScripting;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }
    public E1_PlayerDetectedState playerDetectedState { get; private set; }
    public E1_ChargeState chargeState { get; private set; }
    public E1_LookForPlayerState lookForPlayerState { get; private set; }
    public E1_MeleeAttackState meleeAttackState{get;private set;}
    public E1_StunState stunState { get; private set; }
    public E1_DeadState deadState { get; private set; }

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
    // private Transform meleeAttackPosition;

    private GameObject meleeAttackCollider;


    public override void Awake()
    {
        base.Awake();
        meleeAttackCollider = transform.Find("MeleeAttackCollider").gameObject;
        Debug.Log("meleeAttackCollider: " + meleeAttackCollider);


        moveState = new E1_MoveState(this, "move", moveStateData, this);
        idleState = new E1_IdleState(this, "idle", idleStateData, this);
        playerDetectedState = new E1_PlayerDetectedState(this, "playerDetected", playerDetectedData, this);
        chargeState = new E1_ChargeState(this, "charge", chargeStateData, this); // was charge
        lookForPlayerState = new E1_LookForPlayerState(this, "lookForPlayer", lookForPlayerStateData, this); // was lookForPlayer
        meleeAttackState = new E1_MeleeAttackState(this, "meleeAttack", meleeAttackCollider, meleeAttackStateData, this);
        stunState = new E1_StunState(this, "stun", stunStateData, this);
        deadState = new E1_DeadState(this, "dead", deadStateData, this);

        stats.Stun.OnCurrentValueZero += HandleStunZero;
        stats.Health.OnValueChange += HandleDamageTaken;
    }

    private void HandleStunZero()
    {
        // Debug.Log("HandleStunZero");
        stateMachine.ChangeState(stunState);
    }

    private void HandleDamageTaken()
    {
        // Debug.Log("HandleStunZero");
        stateMachine.ChangeState(lookForPlayerState);
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
