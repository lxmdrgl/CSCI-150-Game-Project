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
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	protected D_MeleeAttack stateData;

	PolygonCollider2D hitbox;  
	private List<Collider2D> detected = new List<Collider2D>();

	public MeleeAttackState(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData) : base(entity, animBoolName, meleeAttackCollider) 
	{
		this.stateData = stateData;
	}
	
	public override void TriggerAttack() 
	{
		base.TriggerAttack();

    	// Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

		hitbox = meleeAttackCollider.gameObject.GetComponent<PolygonCollider2D>();

		hitbox.enabled = true;

		Physics2D.OverlapCollider(hitbox, detected);

		Debug.Log("Detected: " + detected.ToArray() + "count: " +  detected.Count);

    	// Use the TryDamage utility to apply damage to detected objects
    	if (CombatDamageUtilities.TryDamage(detected.ToArray(), new DamageData(stateData.attackDamage, core.Root), out var damageables))
    	{
        	foreach (var damageable in damageables)
        	{
            	Debug.Log("Enemy Dealing " + stateData.attackDamage + " Damage To Player");
        	}
    	}
    	else
    	{
        	Debug.Log("No damageable objects detected");
    	}

		bool didKnock = CombatKnockBackUtilities.TryKnockBack(detected.ToArray(), new KnockBackData(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection, core.Root), out _);
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
