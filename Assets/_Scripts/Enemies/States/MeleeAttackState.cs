﻿using System;
using System.Collections;
using System.Collections.Generic;
using Game.Combat.Damage;
//using Game.Combat.KnockBack;
//using Game.Combat.PoiseDamage;
using Game.CoreSystem;
using UnityEngine;
using Game.Utilities;

public class MeleeAttackState : AttackState 
{
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	protected D_MeleeAttack stateData;

	public MeleeAttackState(Entity entity, string animBoolName, Transform attackPosition, D_MeleeAttack stateData) : base(entity, animBoolName, attackPosition) 
	{
		this.stateData = stateData;
	}
	
	public override void TriggerAttack() 
	{
		base.TriggerAttack();

    	Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

    	// Use the TryDamage utility to apply damage to detected objects
    	if (CombatDamageUtilities.TryDamage(detectedObjects, new DamageData(stateData.attackDamage, core.Root), out var damageables))
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

		/*
		base.TriggerAttack();

		Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);

		foreach (Collider2D collider in detectedObjects) 
		{
			IDamageable damageable = collider.GetComponent<IDamageable>();

			if (damageable != null) 
			{
				Debug.Log("Dealing Damage");
				damageable.Damage(new DamageData(stateData.attackDamage, core.Root));
			}


			/*
			IKnockBackable knockBackable = collider.GetComponent<IKnockBackable>();

			if (knockBackable != null) 
			{
				knockBackable.KnockBack(new KnockBackData(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection, core.Root));
			}

			if (collider.TryGetComponent(out IPoiseDamageable poiseDamageable))
			{
				poiseDamageable.DamagePoise(new PoiseDamageData(stateData.PoiseDamage, core.Root));
			}
		}
		*/
	}
}
