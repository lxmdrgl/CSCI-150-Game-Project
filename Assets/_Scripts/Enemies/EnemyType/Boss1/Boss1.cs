using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Boss1 : Entity
{
    public Boss1_Idle idleState { get; private set; }
    public Boss1_Pursuit chargeState { get; private set; }
    public Boss1_MeleeAttack meleeAttackState{get;private set;}
    public Boss1_Dead deadState { get; private set; }
    public Boss1_Cooldown cooldownState { get; private set; }
    public Boss1_Stomp stompAttackState { get; private set; }

    public float stompRange;
    public float swingRange;
    
    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_MeleeAttack stompAttackData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_CooldownState cooldownStateData;
    public GameObject meleeAttackCollider;
    public GameObject stompAttackCollider;

    public enum LastAttackType
    {
        None,
        Swing,
        Stomp
    }
    public LastAttackType lastAttackType = LastAttackType.None;
    public GameObject spikePrefab;
    public float delayBetweenSteps = 0.25f;
    public int steps = 5;
    public float spacing = 2f;
    public GameObject stalactitePrefab;
    public Transform spawnAreaLeft;
    public Transform spawnAreaRight;
    public int stalactitesPerAttack = 5;
    public override void Awake()
    {
        base.Awake();

        idleState = new Boss1_Idle(this, "idle", idleStateData, this);
        chargeState = new Boss1_Pursuit(this, "charge", chargeStateData, this); 
        meleeAttackState = new Boss1_MeleeAttack(this, "meleeAttack", meleeAttackCollider, meleeAttackStateData, this);
        deadState = new Boss1_Dead(this, "dead", deadStateData, this);
        cooldownState = new Boss1_Cooldown(this, "cooldown", cooldownStateData, this);
        stompAttackState = new Boss1_Stomp(this, "stompAttack", stompAttackCollider, stompAttackData, this);

        stats.Stun.OnCurrentValueZero += HandleStunZero;
        stats.Health.OnValueChange += HandleDamageTaken;
    }

    private void HandleStunZero()
    {
        // Debug.Log("HandleStunZero");
        //stateMachine.ChangeState(stunState);
        //stats.Stun.CurrentValue = stats.Stun.MaxValue;
    }

    private void HandleDamageTaken()
    {
        if(stateMachine.currentState == idleState)
        {
            stateMachine.ChangeState(chargeState);
        }
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void OnDisable() {
        stats.Stun.OnCurrentValueZero -= HandleStunZero;
        stats.Health.OnValueChange -= HandleDamageTaken;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stompRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, swingRange);
    }

    public IEnumerator SpawnSpikes()
    {
        for (int i = 1; i <= steps; i++)
        {
            float offset = spacing * i;
            Vector3 spawnHeightOffset = Vector3.up * 0.75f;

            // Left spike (normal rotation)
            Instantiate(spikePrefab, transform.position + Vector3.left * offset + spawnHeightOffset, Quaternion.identity);

            // Right spike (flipped 180° on Y-axis)
            Quaternion flipY = Quaternion.Euler(0, 180f, 0);
            Instantiate(spikePrefab, transform.position + Vector3.right * offset + spawnHeightOffset, flipY);

            yield return new WaitForSeconds(delayBetweenSteps);
        }
    }


    public float minSpacing = 1.5f;
    public float minGravity = 3f;
    public float maxGravity = 7f;
    public void SpawnStalactites()
    {
        List<float> usedX = new List<float>();

        float baseY = spawnAreaLeft.position.y;

        // 1. Spawn directly above the player
        float playerX = targetPlayer.position.x;
        float clampedX = Mathf.Clamp(playerX, spawnAreaLeft.position.x, spawnAreaRight.position.x);
        usedX.Add(clampedX);

        GameObject playerStalactite = Instantiate(stalactitePrefab, new Vector2(clampedX, baseY), Quaternion.identity);
        SetRandomGravity(playerStalactite);

        // 2. Spawn the rest randomly without stacking
        for (int i = 1; i < stalactitesPerAttack; i++)
        {
            int attempts = 0;
            float xPos = 0f;
            bool valid = false;

            while (attempts < 10 && !valid)
            {
                xPos = Random.Range(spawnAreaLeft.position.x, spawnAreaRight.position.x);
                valid = true;

                foreach (float used in usedX)
                {
                    if (Mathf.Abs(xPos - used) < minSpacing)
                    {
                        valid = false;
                        break;
                    }
                }

                attempts++;
            }

            if (valid)
            {
                usedX.Add(xPos);
                GameObject stalactite = Instantiate(stalactitePrefab, new Vector2(xPos, baseY), Quaternion.identity);
                SetRandomGravity(stalactite);
            }
        }
    }

    private void SetRandomGravity(GameObject obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = Random.Range(minGravity, maxGravity);
        }
    }

    public SpriteRenderer targetRenderer;           // Drag your boss's SpriteRenderer here
    public Light2D spriteLight2D;                   // Drag your Sprite Light 2D here

    void LateUpdate()
    {
        if (targetRenderer != null && spriteLight2D != null)
        {
            spriteLight2D.lightCookieSprite = targetRenderer.sprite;
        }
    }
}
