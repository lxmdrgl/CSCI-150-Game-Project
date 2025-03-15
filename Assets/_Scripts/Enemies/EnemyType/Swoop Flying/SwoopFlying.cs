using Unity.VisualScripting;
using UnityEngine;

public class SwoopFlying : Entity
{
    public SwoopFlying_Idle idleState { get; private set; }
    public SwoopFlying_Pursuit chargeState { get; private set; }
    public SwoopFlying_SwoopAttack meleeAttackState{get;private set;}
    public SwoopFlying_Stun stunState { get; private set; }
    public SwoopFlying_Dead deadState { get; private set; }
    public SwoopFlying_Cooldown cooldownState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
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


        idleState = new SwoopFlying_Idle(this, "idle", idleStateData, this);
        chargeState = new SwoopFlying_Pursuit(this, "charge", chargeStateData, this); 
        meleeAttackState = new SwoopFlying_SwoopAttack(this, "meleeAttack", meleeAttackCollider, meleeAttackStateData, this);
        stunState = new SwoopFlying_Stun(this, "stun", stunStateData, this);
        deadState = new SwoopFlying_Dead(this, "dead", deadStateData, this);
        cooldownState = new SwoopFlying_Cooldown(this, "cooldown", cooldownStateData, this);

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
        if(stateMachine.currentState == idleState)
        {
            stateMachine.ChangeState(chargeState);
        }
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void OnDisable() {
        stats.Stun.OnCurrentValueZero -= HandleStunZero;
        stats.Health.OnValueChange -= HandleDamageTaken;
    }
}
