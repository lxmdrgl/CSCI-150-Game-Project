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
    private D_DeadState deadStateData;
    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        moveState = new E1_MoveState(this, "move", moveStateData, this);
        idleState = new E1_IdleState(this, "idle", idleStateData, this);
        playerDetectedState = new E1_PlayerDetectedState(this, "playerDetected", playerDetectedData, this);
        chargeState = new E1_ChargeState(this, "charge", chargeStateData, this);
        lookForPlayerState = new E1_LookForPlayerState(this, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new E1_MeleeAttackState(this, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        deadState = new E1_DeadState(this, "dead", deadStateData, this);

        // stats.Stun.OnCurrentValueZero += HandleStunZero;
    }

    private void HandlePoiseZero()
    {
        stateMachine.ChangeState(stunState);
    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
    }
}
