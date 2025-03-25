using UnityEngine;
using Game.CoreSystem;

namespace Game.Weapons.Components
{
    public class Parry : WeaponComponent<ParryData, AttackParry>
    {
        private Animator anim;
        private DamageReceiver damageReceiver;
        private KnockBackReceiver knockbackReceiver;
        private PolygonCollider2D hitbox; 

        private bool parry;

        protected override void HandleEnter()
        {
            base.HandleEnter();
        }

        protected override void HandleExit()
        {
            base.HandleExit();

            damageReceiver.SetCanTakeDamage(true);
            knockbackReceiver.SetCanTakeKnockBack(true);
            hitbox.enabled = false;
        }

        private void HandleOnParryActionSetActive(bool parry)
        {
            base.HandleEnter();

            this.parry = parry;

            if (parry)
            {
                damageReceiver.SetCanTakeDamage(false);
                knockbackReceiver.SetCanTakeKnockBack(false);
                hitbox.enabled = true;
                Debug.Log("Parry data: " + currentAttackData);
                hitbox.points = currentAttackData.HitBox.points;
            }
            else
            {
                damageReceiver.SetCanTakeDamage(true);
                knockbackReceiver.SetCanTakeKnockBack(true);
                hitbox.enabled = false;
            }
        }

        private void HandleParry()
        {
            Debug.Log("Ignore damage parry: " + parry);
            if (parry)
            {
                anim.SetTrigger("parry");
            }
        }

        protected override void Start()
        {
            base.Awake();

            anim = GetComponentInChildren<Animator>();
            damageReceiver = Core.GetCoreComponent<DamageReceiver>();
            knockbackReceiver = Core.GetCoreComponent<KnockBackReceiver>();
            hitbox = weapon.WeaponPlayerColliderGameObject.gameObject.GetComponent<PolygonCollider2D>();

            AnimationEventHandler.OnParryActionSetActive += HandleOnParryActionSetActive;
            damageReceiver.OnIgnoreDamage += HandleParry;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            AnimationEventHandler.OnParryActionSetActive -= HandleOnParryActionSetActive;
            damageReceiver.OnIgnoreDamage -= HandleParry;
        }
    }
}