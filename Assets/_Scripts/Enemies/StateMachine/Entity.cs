using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
public class Entity : MonoBehaviour, IDataPersistence
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

    [SerializeField] private string id;


    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();

        stats = Core.GetCoreComponent<Stats>();

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

    public virtual void ResetStun() {
		isStunned = false;
		// currentStunResistance = entityData.stunResistance;
	}

        // For saving & loading
    [ContextMenu("Generate Guid For Id")]
    private void GenerateGuid()
    {
        id=System.Guid.NewGuid().ToString();
    }
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public void LoadData(GameData data)
    {

        if (data == null)
        {
            Debug.LogError("GameData is null. Cannot load data.");
            return;
        }
        
        // Find the enemy data in the list using this entity's unique id
        GameData.EnemyData? enemyData = data.enemyData.Find(e => e.Guid == id);

        if (enemyData.HasValue)
        {
            // Convert Vector2Data to Vector2
            Position = enemyData.Value.enemyPosition.ToVector2();
            stats.Health.CurrentValue = enemyData.Value.enemyCurrentHp;
            stats.Health.MaxValue = enemyData.Value.enemyMaxHp;
            gameObject.SetActive(enemyData.Value.isAlive); // Toggle based on saved isAlive status
        }
    }

    public void SaveData(GameData data)
    {
        // Create a new EnemyData instance with updated values
        GameData.EnemyData enemySaveData = new GameData.EnemyData(
            stats.Health.CurrentValue,
            stats.Health.MaxValue,
            new GameData.Vector2Data(Position), // Convert Vector2 to Vector2Data
            stats.Health.CurrentValue > 0, // Determine if the enemy is alive
            id
        );

        // Check if this enemy's data already exists in the list and update it if so
        int index = data.enemyData.FindIndex(e => e.Guid == id);
        if (index >= 0)
        {
            data.enemyData[index] = enemySaveData;
        }
        else
        {
            data.enemyData.Add(enemySaveData);
        }
    }
    
}
