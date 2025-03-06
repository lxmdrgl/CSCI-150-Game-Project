using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

using Game.CoreSystem;
using Game.Weapons;
using Game.Utilities;
using System;

public class Player : MonoBehaviour, IDataPersistence
{
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerNormalAirState AirState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState PrimaryAttackHoldState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }
    public PlayerAttackState PrimarySkillState { get; private set; }
    public PlayerAttackState SecondarySkillState { get; private set; }
    public PlayerKnockBackState KnockBackState { get; private set; }
    public PlayerPlatformAirState PlatformAirState { get; private set; }


    [SerializeField]
    public PlayerData playerData;

    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Collider2D boxCollider { get; private set; }

    public InteractableDetector InteractableDetector { get; private set; }

    protected Stats stats;
    protected KnockBackReceiver knockBackReceiver;

    private Vector2 workspace; 

    private Weapon primaryAttack;
    private Weapon primaryAttackHold;
    private Weapon secondaryAttack;
    private Weapon primarySkill;
    private Weapon secondarySkill;

    public TimeNotifier dashTimeNotifier;
    public TimeNotifier primarySkillTimeNotifier;
    public TimeNotifier secondarySkillTimeNotifier;

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        stats = Core.GetCoreComponent<Stats>();
        knockBackReceiver = Core.GetCoreComponent<KnockBackReceiver>();

        primaryAttack = transform.Find("PrimaryAttack").GetComponent<Weapon>();
        primaryAttackHold = transform.Find("PrimaryAttackHold").GetComponent<Weapon>();
        secondaryAttack = transform.Find("SecondaryAttack").GetComponent<Weapon>();
        primarySkill = transform.Find("PrimarySkill").GetComponent<Weapon>();
        secondarySkill = transform.Find("SecondarySkill").GetComponent<Weapon>();

        primaryAttack.SetCore(Core);
        primaryAttackHold.SetCore(Core);
        secondaryAttack.SetCore(Core);
        primarySkill.SetCore(Core);
        secondarySkill.SetCore(Core);

        InteractableDetector = Core.GetCoreComponent<InteractableDetector>();


        StateMachine = new PlayerStateMachine();

        dashTimeNotifier = new TimeNotifier();
        primarySkillTimeNotifier = new TimeNotifier();
        secondarySkillTimeNotifier = new TimeNotifier();

        IdleState = new PlayerIdleState(this, "idle");
        MoveState = new PlayerMoveState(this, "move");
        JumpState = new PlayerJumpState(this, "air"); // was jump
        DashState = new PlayerDashState(this, "idle"); // needs to be dash 
        AirState = new PlayerNormalAirState(this, "air");
        WallGrabState = new PlayerWallGrabState(this, "wallGrab");
        WallJumpState = new PlayerWallJumpState(this, "air"); // was jump
        PrimaryAttackState = new PlayerAttackState(this, "attack", primaryAttack, CombatInputs.primaryAttackPress);
        PrimaryAttackHoldState = new PlayerAttackState(this, "attack", primaryAttackHold, CombatInputs.primaryAttackHold);
        SecondaryAttackState = new PlayerAttackState(this, "attack", secondaryAttack, CombatInputs.secondaryAttackPress);
        PrimarySkillState = new PlayerAttackState(this, "attack", primarySkill, CombatInputs.primarySkillPress);
        SecondarySkillState = new PlayerAttackState(this, "attack", secondarySkill, CombatInputs.secondarySkillPress);
        KnockBackState = new PlayerKnockBackState(this, "knockBack");
        PlatformAirState = new PlayerPlatformAirState(this, "air");

        knockBackReceiver.OnKnockBackActive += HandleKnockBackActive;
    }

    private void HandleKnockBackActive()
    {
        StateMachine.ChangeState(KnockBackState);
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();

        InputHandler.OnInteractInputChanged += InteractableDetector.TryInteract;


        RB = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<Collider2D>();

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

    public virtual void OnEnable()
    {
        dashTimeNotifier.OnNotify += DashState.ResetDashCooldown;
        primarySkillTimeNotifier.OnNotify += PrimarySkillState.ResetAttackCooldown;
        secondarySkillTimeNotifier.OnNotify += SecondarySkillState.ResetAttackCooldown;
        
    }

    public virtual void OnDisable()
    {
        dashTimeNotifier.OnNotify -= DashState.ResetDashCooldown;
        primarySkillTimeNotifier.OnNotify -= PrimarySkillState.ResetAttackCooldown;
        secondarySkillTimeNotifier.OnNotify -= SecondarySkillState.ResetAttackCooldown;
        
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    // For saving & loading
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public void LoadData(GameData data)
    {
        stats.Health.CurrentValue = data.playerCurrentHp;
        stats.Health.MaxValue = data.playerMaxHp;
        Position = data.playerPosition.ToVector2();
    }
    public void SaveData(GameData data)
    {
        data.playerCurrentHp = stats.Health.CurrentValue;
        data.playerMaxHp = stats.Health.MaxValue;
        data.playerPosition = new GameData.Vector2Data(Position);
        data.runTime += Time.timeSinceLevelLoad;
        data.currentLevelTime = Time.timeSinceLevelLoad;
    }
    public void SaveSaveData(SaveData data)
    {
        
    }
    public void LoadSaveData(SaveData data)
    {
    
    }
}
