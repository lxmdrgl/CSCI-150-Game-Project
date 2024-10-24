using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }
    public E1_PlayerDetectedState playerDetectedState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;

    public override void Awake()
    {
        base.Awake();

        moveState = new E1_MoveState(this, "move", moveStateData, this);
        idleState = new E1_IdleState(this, "idle", idleStateData, this);
        playerDetectedState = new E1_PlayerDetectedState(this, "playerDetected", playerDetectedData, this);
    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
    }
}
