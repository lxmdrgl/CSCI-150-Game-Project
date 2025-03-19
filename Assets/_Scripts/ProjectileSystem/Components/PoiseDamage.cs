using Game.Combat.StunDamage;
using Game.Interfaces;
using Game.ProjectileSystem.DataPackages;
using Game.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Game.ProjectileSystem.Components
{
    /*
     * The StunDamage component is responsible for using information provided by the HitBox component to damage the Stun of any entities that are on the relevant LayerMask
     * The amount comes from the weapon via the ProjectileDataPackage system.
     */
    public class StunDamage : ProjectileComponent
    {
        public UnityEvent OnStunDamage;

        [field: SerializeField] public LayerMask LayerMask { get; private set; }
        
        private float amount;

        private HitBox hitBox;
        
        private void HandleRaycastHit2D(RaycastHit2D[] hits)
        {
            if (!Active)
                return;

            foreach (var hit in hits)
            {
                // Is the object under consideration part of the LayerMask that we can damage?
                if (!LayerMaskUtilities.IsLayerInMask(hit, LayerMask))
                    continue;

                // NOTE: We need to use .collider.transform instead of just .transform to get the GameObject the collider we detected is attached to, otherwise it returns the parent
                if (!hit.collider.transform.gameObject.TryGetComponent(out IStunDamageable StunDamageable))
                    continue;
                
                StunDamageable.DamageStun(new StunDamageData(amount, projectile.gameObject));
                
                OnStunDamage?.Invoke();

                return;
            }
        }
        
        // Handles checking to see if the data is relevant or not, and if so, extracts the information we care about
        protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
            base.HandleReceiveDataPackage(dataPackage);

            if (dataPackage is not StunDamageDataPackage package)
                return;

            amount = package.Amount;
        }
        
        #region Plumbing

        protected override void Awake()
        {
            base.Awake();

            hitBox = GetComponent<HitBox>();

            hitBox.OnRaycastHit2D.AddListener(HandleRaycastHit2D);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnRaycastHit2D.RemoveListener(HandleRaycastHit2D);
        }

        #endregion
    }
}