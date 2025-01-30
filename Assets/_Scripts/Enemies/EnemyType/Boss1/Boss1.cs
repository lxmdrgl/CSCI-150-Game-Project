using Unity.VisualScripting;
using UnityEngine;

public class Boss1 : Entity
{
    public Boss1_Idle idleState { get; private set; }
    public Boss1_Pursuit chargeState { get; private set; }
    public Boss1_MeleeAttack meleeAttackState{get;private set;}
    public Boss1_Dead deadState { get; private set; }
    public Boss1_Cooldown cooldownState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_CooldownState cooldownStateData;

    public Transform player; // Reference to the player's transform
    public Rigidbody2D rb;

    private GameObject meleeAttackCollider;


    public override void Awake()
    {
        base.Awake();
        meleeAttackCollider = transform.Find("MeleeAttackCollider").gameObject;
        Debug.Log("meleeAttackCollider: " + meleeAttackCollider);


        idleState = new Boss1_Idle(this, "idle", idleStateData, this);
        chargeState = new Boss1_Pursuit(this, "charge", chargeStateData, this); 
        meleeAttackState = new Boss1_MeleeAttack(this, "meleeAttack", meleeAttackCollider, meleeAttackStateData, this);
        deadState = new Boss1_Dead(this, "dead", deadStateData, this);
        cooldownState = new Boss1_Cooldown(this, "cooldown", cooldownStateData, this);

        stats.Stun.OnCurrentValueZero += HandleStunZero;
        stats.Health.OnValueChange += HandleDamageTaken;

        rb = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene! Make sure the player GameObject is tagged as 'Player'.");
        }
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
}
