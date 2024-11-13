using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
public class Entity : MonoBehaviour
{
    private Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }
	private Movement movement;
    public EnemyStateMachine stateMachine;
    public D_Entity entityData;

    public Animator anim { get; private set; }
    public AnimationToStatemachine atsm { get; private set; }
    public Core Core { get; private set; }

    [SerializeField]
	private Transform playerCheck;

    private float currentStunResistance;

    private Vector2 velocityWorkspace;

    protected bool isStunned;

    protected Stats stats;

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
        anim = GetComponent<Animator>();
		atsm = GetComponent<AnimationToStatemachine>();
        stateMachine = new EnemyStateMachine();
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
        anim.SetFloat("yVelocity", Movement.RB.linearVelocity.y);
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        Debug.DrawRay(playerCheck.position, transform.right * entityData.minAgroDistance, Color.red);

        // Cast the ray to check for both the player and obstacles
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer | entityData.whatIsGround);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            // If it hit a player, return true
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        // If it hit nothing or hit an obstacle first, return false
        return false;
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction() {
		return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
	}

    public virtual void ResetStunResistance() {
		isStunned = false;
		currentStunResistance = entityData.stunResistance;
	}
}
