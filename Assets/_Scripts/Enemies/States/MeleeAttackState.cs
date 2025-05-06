using System;
using System.Collections;
using System.Collections.Generic;
using Game.Combat.Damage;
using Game.Combat.KnockBack;
using Game.Combat.StunDamage;
using Game.CoreSystem;
using UnityEngine;
using Game.Utilities;
using Unity.VisualScripting;

public class MeleeAttackState : AttackState 
{
	protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
	private Stats Stats { get => stats ?? core.GetCoreComponent(ref stats); }
	private CollisionSenses collisionSenses;
	protected D_MeleeAttack stateData;
	PolygonCollider2D hitbox;  
	private List<Collider2D> detected = new List<Collider2D>();

	public MeleeAttackState(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData) : base(entity, animBoolName, meleeAttackCollider) 
	{
		this.stateData = stateData;
	}
	
	public override bool TriggerAttack() 
	{
		base.TriggerAttack();

    	// Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

		hitbox = meleeAttackCollider.gameObject.GetComponent<PolygonCollider2D>();

		hitbox.enabled = true;

		Physics2D.OverlapCollider(hitbox, detected);

		// test Debug.Log("Detected: " + detected.ToArray() + "count: " +  detected.Count);

    	// Use the TryDamage utility to apply damage to detected objects
		if (Stats == null) {
			// test Debug.LogError("Stats is null");
		}

		bool didDamage = CombatDamageUtilities.TryDamage(detected.ToArray(), new DamageData(stateData.attackDamage * (Stats.Attack / 100f), core.Root), out var damageables);
    	if (didDamage)
    	{
        	foreach (var damageable in damageables)
        	{
            	// test Debug.Log("Enemy Dealing " + stateData.attackDamage + " Damage To Player");
        	}
    	}
    	else
    	{
        	// test Debug.Log("No damageable objects detected");
    	}


		bool didKnock = CombatKnockBackUtilities.TryKnockBack(detected.ToArray(), new KnockBackData(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection, core.Root), out _);
		if (didKnock) {
			// test Debug.Log("Enemy Knocking Player Back");
		}
		else {
			// // test Debug.Log("No knockbackable objects detected");
		}

		if (didDamage || didKnock)
		{
			return true;
		}

		return false;
	}

    public override void FinishAttack()
    {
        base.FinishAttack();

		hitbox.enabled = false;
    }

	public virtual void DisableHitbox() 
	{
		if (hitbox) {
			hitbox.enabled = false;
		}
	} 
}
