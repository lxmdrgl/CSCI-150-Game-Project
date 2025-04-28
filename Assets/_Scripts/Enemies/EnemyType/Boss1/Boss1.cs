using Unity.VisualScripting;
using UnityEngine;

public class Boss1 : Entity
{
    public Boss1_Idle idleState { get; private set; }
    public Boss1_Pursuit chargeState { get; private set; }
    public Boss1_MeleeAttack meleeAttackState{get;private set;}
    public Boss1_Dead deadState { get; private set; }
    public Boss1_Cooldown cooldownState { get; private set; }
    public Boss1_Stomp stompAttackState { get; private set; }

    public float stompRange;
    public float swingRange;
    
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_MeleeAttack stompAttackData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_CooldownState cooldownStateData;
    public GameObject meleeAttackCollider;
    public GameObject stompAttackCollider;

    public enum LastAttackType
    {
        None,
        Swing,
        Stomp
    }

    public LastAttackType lastAttackType = LastAttackType.None;


    public override void Awake()
    {
        base.Awake();

        idleState = new Boss1_Idle(this, "idle", idleStateData, this);
        chargeState = new Boss1_Pursuit(this, "charge", chargeStateData, this); 
        meleeAttackState = new Boss1_MeleeAttack(this, "meleeAttack", meleeAttackCollider, meleeAttackStateData, this);
        deadState = new Boss1_Dead(this, "dead", deadStateData, this);
        cooldownState = new Boss1_Cooldown(this, "cooldown", cooldownStateData, this);
        stompAttackState = new Boss1_Stomp(this, "stompAttack", stompAttackCollider, stompAttackData, this);

        stats.Stun.OnCurrentValueZero += HandleStunZero;
        stats.Health.OnValueChange += HandleDamageTaken;
    }

    private void HandleStunZero()
    {
        // Debug.Log("HandleStunZero");
        //stateMachine.ChangeState(stunState);
        //stats.Stun.CurrentValue = stats.Stun.MaxValue;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stompRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, swingRange);
    }
}
