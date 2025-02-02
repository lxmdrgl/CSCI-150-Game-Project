using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

namespace Game.CoreSystem
{
    public class CollisionSenses : CoreComponent
    {
        private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }

		private Movement movement;

        public Transform GroundCheck 
        {
			get => GenericNotImplementedError<Transform>.TryGet(groundCheck, core.transform.parent.name);
			private set => groundCheck = value;
		}
        public Transform WallCheck 
        {
			get => GenericNotImplementedError<Transform>.TryGet(wallCheck, core.transform.parent.name);
			private set => wallCheck = value;
		}
        public Transform LedgeCheckVertical {
			get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckVertical, core.transform.parent.name);
			private set => ledgeCheckVertical = value;
		}

        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Vector2 groundCheckSize;
        [SerializeField] private float wallCheckDistance;
        // [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private string platformTag;
        [SerializeField] private Transform ledgeCheckVertical;

        [SerializeField] private Transform platformTopRight;
        [SerializeField] private Transform platformMidLeft;
        [SerializeField] private Transform platformBottomLeft;

        // [SerializeField] private Collider2D collidedPlatform;

        public bool Ground 
        {
            get => Physics2D.OverlapBox(GroundCheck.position, groundCheckSize, 0f, whatIsGround);
		}

        public Collider2D PlatformDown
        {
            get 
            {
                Collider2D collider = Physics2D.OverlapBox(GroundCheck.position, groundCheckSize, 0f, whatIsGround);
                if (collider != null && collider.CompareTag(platformTag)) {
                    return collider;
                } else {
                    return null;
                }
            }
            
            
        }

        public Collider2D PlatformOverlapBottom
        {
            get
            {
                Collider2D collider = Physics2D.OverlapArea(platformBottomLeft.position, platformTopRight.position, whatIsGround);
                if (collider != null && collider.CompareTag(platformTag)) {
                    // collidedPlatform = collider;
                    return collider;
                } else {
                    return null;
                }
            } 
        }

        public Collider2D PlatformOverlapTop
        {
            get
            {
                Collider2D collider = Physics2D.OverlapArea(platformMidLeft.position, platformTopRight.position, whatIsGround);
                if (collider != null && collider.CompareTag(platformTag)) {
                    // collidedPlatform = collider;
                    return collider;
                } else {
                    return null;
                }
            } 
        }



        public bool WallFront 
        {
			get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
		}

        public bool WallBack 
        {
			get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround);
		}
        public bool LedgeVertical {
			get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround);
		}

        private void OnDrawGizmos()
        {
            /* Gizmos.color = Color.red;
            if (platformTopRight == null || platformBottomLeft == null) return;
            Gizmos.DrawWireCube(platformTopRight.position, new Vector3(0.05f, 0.05f, 0f));
            Gizmos.DrawWireCube(platformBottomLeft.position, new Vector3(0.05f, 0.05f, 0f)); */

            /* if (collidedPlatform != null) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(collidedPlatform.bounds.center, collidedPlatform.bounds.size);
            } */
        }
    }
}


