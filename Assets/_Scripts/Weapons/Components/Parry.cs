using UnityEngine;
using Game.CoreSystem;

namespace Game.Weapons.Components
{
    public class Parry : WeaponComponent
    {
        private Animator anim;
        private DamageReceiver damageReceiver;
        private KnockBackReceiver knockbackReceiver;

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
        }

        private void HandleOnParryActionSetActive(bool parry)
        {
            this.parry = parry;

            if (parry)
            {
                damageReceiver.SetCanTakeDamage(false);
                knockbackReceiver.SetCanTakeKnockBack(false);
            }
            else
            {
                damageReceiver.SetCanTakeDamage(true);
                knockbackReceiver.SetCanTakeKnockBack(true);
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

        protected override void Awake()
        {
            base.Awake();

            anim = GetComponentInChildren<Animator>();
            damageReceiver = Core.GetCoreComponent<DamageReceiver>();
            knockbackReceiver = Core.GetCoreComponent<KnockBackReceiver>();

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