/*using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveEnemy : Entity
{
    public EE_IdleState idleState { get; private set; }
    public EE_MoveState moveState { get; private set; }
    public EE_PlayerDetectedState playerDetectedState { get; private set; }
    public EE_ChargeState chargeState { get; private set; }
    public EE_LookForPlayerState lookForPlayerState { get; private set; }
    public EE_MeleeAttackState meleeAttackState{get;private set;}
    public EE_StunState stunState { get; private set; }
    public EE_DeadState deadState { get; private set; }
    public EE_CooldownState cooldownState { get; private set; }
    public EE_ExplosiveAttackState rangedAttackState { get; private set; }
    

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
    private D_ExplosiveAttackState explosiveAttackStateData;

    private GameObject meleeAttackCollider;

    [SerializeField]
    private Transform rangedAttackPosition;
    [SerializeField]
    private Transform radialRangedAttackPosition;

    public Transform player; // Reference to the player's transform
    public Rigidbody2D rb;
    public override void Awake()
    {
        base.Awake();
        meleeAttackCollider = transform.Find("MeleeAttackCollider").gameObject;
        // Debug.Log("meleeAttackCollider: " + meleeAttackCollider);


        moveState = new EE_MoveState(this, "move", moveStateData, this);
        idleState = new EE_IdleState(this, "idle", idleStateData, this);
        playerDetectedState = new EE_PlayerDetectedState(this, "playerDetected", playerDetectedData, this);
        chargeState = new EE_ChargeState(this, "charge", chargeStateData, this); // was charge
        lookForPlayerState = new EE_LookForPlayerState(this, "lookForPlayer", lookForPlayerStateData, this); // was lookForPlayer
        meleeAttackState = new EE_MeleeAttackState(this, "meleeAttack", meleeAttackCollider, meleeAttackStateData, this);
        stunState = new EE_StunState(this, "stun", stunStateData, this);
        deadState = new EE_DeadState(this, "dead", deadStateData, this);
        cooldownState = new EE_CooldownState(this, "cooldown", cooldownStateData, this);
        rangedAttackState = new EE_ExplosiveAttackState(this, "explosiveAttack", rangedAttackPosition, explosiveAttackStateData, this );
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
*/