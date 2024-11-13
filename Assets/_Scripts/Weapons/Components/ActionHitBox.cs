using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;

namespace Game.Weapons.Components
{
    public class ActionHitBox : WeaponComponent<ActionHitBoxData, AttackActionHitBox>
    {
        public event Action<Collider2D[]> OnDetectedCollider2D;

        private CoreComp<CoreSystem.Movement> movement;

        private Vector2 offset;

        ContactFilter2D filter;

        PolygonCollider2D hitbox;        
        private List<Collider2D> detected = new List<Collider2D>();

        private void HandleAttackAction()
        {
            /* offset.Set(
                transform.position.x + (currentAttackData.HitBox.center.x * movement.Comp.FacingDirection),
                transform.position.y + currentAttackData.HitBox.center.y
            ); */
            // detected = Physics2D.OverlapBoxAll(offset, currentAttackData.HitBox.size, 0f, data.DetectableLayers);
            // filter.SetLayerMask(data.DetectableLayers);
            // filter.useLayerMask = true;

            // currentAttackData.HitBox.Overlap(filter, detected);
            hitbox.enabled = true;
            hitbox.points = currentAttackData.HitBox.points;

            Debug.Log($"Weapon points: {currentAttackData.HitBox}");

            Physics2D.OverlapCollider(hitbox, detected);

            Debug.Log($"Detected: {detected}, count: {detected.Count}");

            if (detected.Count == 0)
                return;

            OnDetectedCollider2D?.Invoke(detected.ToArray());
        }

        protected override void HandleExit()
        {
            hitbox.enabled = false;
        }

        protected override void Start()
        {
            base.Start();

            movement = new CoreComp<CoreSystem.Movement>(Core);
            
            AnimationEventHandler.OnAttackAction += HandleAttackAction;

            hitbox = weapon.WeaponColliderGameObject.gameObject.GetComponent<PolygonCollider2D>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AnimationEventHandler.OnAttackAction -= HandleAttackAction;
        }

        /* private void OnDrawGizmosSelected()
        {
            if (data == null)
                return;

            foreach (var item in data.GetAllAttackData())
            {
                if (!item.Debug)
                    continue;
                
                Gizmos.DrawWireCube(transform.position + (Vector3)item.HitBox.center, item.HitBox.size);
            }
        } */
    }
}