using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
public class Entity : MonoBehaviour, IDataPersistence
{
    private Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }
	private Movement movement;
    public EnemyStateMachine stateMachine;
    //[SerializeField] private bool isRangedEnemy;
    //public bool IsRangedEnemy => isRangedEnemy;
    public D_Entity entityData;
    private DamageFlash damageFlash;
    public Animator anim { get; private set; }
    public AnimationToStatemachine atsm { get; private set; }
    public Core Core { get; private set; }

    [SerializeField]
	public Transform playerCheck;
    public List<GameObject> playersInRange;
    public Transform playerPosition1, playerPosition2;
    public Transform targetPlayer;
    public bool isPlayerInPursuitRange;
    private float currentStunResistance;

    private Vector2 velocityWorkspace;

    protected bool isStunned;

    protected Stats stats;
    public CircleCollider2D pursuitRangeCollider;
    public string UniqueId;
    public void GenerateGuid()
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

        damageFlash = GetComponent<DamageFlash>();

        pursuitRangeCollider.radius = entityData.pursuitRange;
        
        playersInRange = new List<GameObject>();
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

    public virtual bool CheckPlayerInAgroRange()
    {
        // Perform a raycast to check for the player or obstacles
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, transform.right, entityData.agroRange, entityData.whatIsPlayer | entityData.whatIsGround);
        
        // Debug the raycast (you can visualize it in the Scene view)
        Debug.DrawRay(playerCheck.position, transform.right * entityData.agroRange, Color.red);
        
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

    public void SetDependencies()
    {
        // Find all players in the scene
        PlayerInput[] players = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);

        // Clear the playersInRange list to avoid duplicates
        playersInRange.Clear();

        // Store each player's GameObject and position
        foreach (PlayerInput player in players)
        {
            if (!playersInRange.Contains(player.gameObject))
            {
                playersInRange.Add(player.gameObject);

                // Store player positions in playerPosition1 and playerPosition2
                if (playersInRange.Count == 1)
                {
                    playerPosition1 = player.transform;
                }
                else if (playersInRange.Count == 2)
                {
                    playerPosition2 = player.transform;
                }
            }
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPursuitRange = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInPursuitRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInPursuitRange = false;
        }
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

    public virtual bool CheckPlayerInCloseRangeAction() 
    {
		return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
	}

    private void OnDamageTaken() 
    {
        // Damage Numbers
        int damageAmount = Mathf.RoundToInt(stats.Health.damageTaken);
        Vector3 damageNumPos = gameObject.GetComponentInChildren<Canvas>().transform.position;
        damageNumPos.y -= 0.5f;
        damageNumPos.x += UnityEngine.Random.Range(-0.5f, 0.5f); // Random x offset
        damageNumPos.y += UnityEngine.Random.Range(-0.5f, 0.5f); // Random y offset
        DamagePopup.Create(damageNumPos,damageAmount,false);
        // Damage Flash Effect
        if(damageFlash)
        {
            damageFlash.CallDamageFlash();
        }
    }

    private void OnEnable()
    {
        stats.Health.OnValueChange += OnDamageTaken;
    }

    private void OnDisable()
    {
        stats.Health.OnValueChange -= OnDamageTaken;
    }

    public virtual void ResetStun() 
    {
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
        if(DataPersistenceManager.instance.disableDataPersistence)
        {
            return;
        }

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

        if(DataPersistenceManager.instance.disableDataPersistence)
        {
            return;
        }

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
    
    public void SaveSaveData(SaveData data)
    {
 
    }
    public void LoadSaveData(SaveData data)
    {
 
    }
}
