using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EnemyStateMachine stateMachine;

    public int facingDirection {  get; private set; }
    public Rigidbody2D rb {  get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO {  get; private set; }

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;

    private Vector2 velocityWorkspace;

    public virtual void Start()
    {
        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent <Animator>(); 

        stateMachine = new EnemyStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.linearVelocity.y);
        rb.linearVelocity = velocityWorkspace;
    }

    public virtual void CheckWall()
    {

    }

    public virtual void CheckLedge()
    {

    }
}
