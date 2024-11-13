using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

using Game.CoreSystem;
using Game.Weapons;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }


    [SerializeField]
    public PlayerData playerData;

    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public InteractableDetector InteractableDetector { get; private set; }


    private Vector2 workspace; 

    private Weapon primaryAttack;
    private Weapon secondaryAttack;

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        primaryAttack = transform.Find("PrimaryAttack").GetComponent<Weapon>();
        secondaryAttack = transform.Find("SecondaryAttack").GetComponent<Weapon>();

        primaryAttack.SetCore(Core);
        secondaryAttack.SetCore(Core);

        InteractableDetector = Core.GetCoreComponent<InteractableDetector>();


        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, "idle");
        MoveState = new PlayerMoveState(this, "move");
        JumpState = new PlayerJumpState(this, "air"); // was jump
        AirState = new PlayerAirState(this, "air");
        WallGrabState = new PlayerWallGrabState(this, "wallGrab");
        WallJumpState = new PlayerWallJumpState(this, "air"); // was jump
        PrimaryAttackState = new PlayerAttackState(this, "attack", primaryAttack, CombatInputs.primaryAttack);
        SecondaryAttackState = new PlayerAttackState(this, "attack", secondaryAttack, CombatInputs.secondaryAttack);
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();

        InputHandler.OnInteractInputChanged += InteractableDetector.TryInteract;


        RB = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    // For saving & loading
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
}
