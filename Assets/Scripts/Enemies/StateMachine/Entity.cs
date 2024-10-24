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

    // public int facingDirection { get; private set; }
    // public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    // public GameObject aliveGO { get; private set; }
    public Core Core { get; private set; }

    // [SerializeField]
    // private Transform wallCheck;
    // [SerializeField]
    // private Transform ledgeCheck;
    // [SerializeField]
	// private Transform groundCheck;
    [SerializeField]
	private Transform playerCheck;

    private Vector2 velocityWorkspace;

    public virtual void Awake()
    {
        // aliveGO = transform.Find("Alive").gameObject;
        // rb = aliveGO.GetComponent<Rigidbody2D>();
        Core = GetComponentInChildren<Core>();
        // anim = aliveGO.GetComponent<Animator>();
        anim = GetComponent<Animator>();

        stateMachine = new EnemyStateMachine();
    }

    /* public virtual void Start()
    {
        facingDirection = 1;
    } */

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

    /* public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.linearVelocity.y);
        rb.linearVelocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    } */

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    /* public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.Rotate(0f, 180f, 0f);
    } */

    // public virtual void OnDrawGizmos()
    // {
    //     if (Core != null && wallCheck != null && ledgeCheck != null)
    //     {
    //         Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Movement.FacingDirection * entityData.wallCheckDistance));
    //         Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));

    //         // Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
	// 		Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
	// 		Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance), 0.2f);
    //     }
    // }
}
