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

    [SerializeField] private string UniqueId;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        UniqueId = System.Guid.NewGuid().ToString();
    }


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
        // Cast the ray to check for both the player and obstacles
        RaycastHit2D hit = Physics2D.CircleCast(playerCheck.position, 10f, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer | entityData.whatIsGround);
        DebugCircleCast(playerCheck.position, 10f, transform.right, entityData.minAgroDistance);
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

void DebugCircleCast(Vector2 origin, float radius, Vector2 direction, float distance)
{
    // Number of points to approximate the circle
    int segments = 36;
    float angleStep = 360f / segments;

    // Draw the initial circle at the origin
    for (int i = 0; i < segments; i++)
    {
        float angle1 = Mathf.Deg2Rad * (i * angleStep);
        float angle2 = Mathf.Deg2Rad * ((i + 1) * angleStep);

        Vector2 point1 = origin + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * radius;
        Vector2 point2 = origin + new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) * radius;

        Debug.DrawLine(point1, point2, Color.green);
    }

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
    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    public void LoadData(GameData data)
    {
        if (string.IsNullOrEmpty(UniqueId))
        {
            Debug.LogError($"{name} has no UniqueID assigned. Cannot load data.");
            return;
        }
        if (data.enemyData.Count==0)
        {
            return;
        }
        // Find the enemy data in the list using this entity's unique id
        GameData.EnemyData? enemyData = data.enemyData?.Find(e => e.UniqueId == UniqueId);

        if (enemyData.HasValue)
        {
            // Safely use enemyData
            Position = enemyData.Value.Position.ToVector2();
            stats.Health.CurrentValue = enemyData.Value.CurrentHp;
            stats.Health.MaxValue = enemyData.Value.MaxHp;
            gameObject.SetActive(enemyData.Value.IsAlive);
        }
        else
        {
            // Handle the case where enemyData is null or not found
            Debug.LogWarning($"EnemyData not found for UniqueId: {UniqueId}");
        }

    }

    public void SaveData(GameData data)
    {
        if (string.IsNullOrEmpty(UniqueId))
        {
            Debug.LogError($"{name} has no UniqueID assigned. Cannot save data.");
            return;
        }

        GameData.EnemyData enemySaveData = new GameData.EnemyData(
            UniqueId,
            new GameData.Vector2Data(Position), // Convert Vector2 to Vector2Data,
            stats.Health.CurrentValue,
            stats.Health.MaxValue,
            stats.Health.CurrentValue > 0
        );

        int index = data.enemyData.FindIndex(e => e.UniqueId == UniqueId);
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
