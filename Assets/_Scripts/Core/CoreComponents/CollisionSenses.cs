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

        public Transform PlatformCheckBottom 
        {
			get => GenericNotImplementedError<Transform>.TryGet(platformCheckBottom, core.transform.parent.name);
			private set => platformCheckBottom = value;
		}



        [Header("Ground & Wall Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Vector2 groundCheckSize;  // 10/24 - 0.03
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private float longGroundDistance;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private Transform ledgeCheckVertical;

        [Header("Platform Check")]
        [SerializeField] private string platformTag;
        // [SerializeField] private Transform platformCheck;
        [SerializeField] private Transform platformCheckBottom;
        [SerializeField] private Transform platformCheckTop;
        [SerializeField] private float playerHeight; 
        [SerializeField] private float platformCheckBottomDistance; 
        [SerializeField] private float platformCheckTopDistance; 
        [SerializeField] private float platformCheckBottomExtendDistance; 
        [SerializeField] private Transform platformTopRight;
        [SerializeField] private Transform platformMidLeft;
        [SerializeField] private Transform platformBottomLeft;


        public bool Ground 
        {
            get => Physics2D.OverlapBox(GroundCheck.position, groundCheckSize, 0f, whatIsGround);
		}

        public bool WallFront 
        {
			get 
            {
                RaycastHit2D hit = Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
                if (hit.collider != null && hit.collider.tag == "Untagged") {
                    return true;
                } else {
                    // Debug.Log($"PlatformBottom false");
                    return false;
                }
            }
        }

        /* public bool WallBack 
        {
			get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround);
		} */
        public bool LedgeVertical {
			get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, whatIsGround);
		}

        public bool LongGround 
        {
            get => Physics2D.Raycast(GroundCheck.position, Vector2.down, longGroundDistance, whatIsGround);
		}

        public RaycastHit2D PlatformBottom
        {
            get 
            {   
                RaycastHit2D hit = Physics2D.Raycast(platformCheckBottom.position, Vector2.down, platformCheckBottomDistance, whatIsGround);
                if (hit.collider != null && hit.collider.CompareTag(platformTag)) {
                    // Debug.Log($"PlatformBottom true");
                    return hit;
                } else {
                    // Debug.Log($"PlatformBottom false");
                    return new RaycastHit2D();
                }
            }
        }

        public RaycastHit2D PlatformBottomUp
        {
            get 
            {   
                RaycastHit2D hit = Physics2D.Raycast(platformCheckBottom.position, Vector2.up, playerHeight, whatIsGround);
                if (hit.collider != null && hit.collider.CompareTag(platformTag)) {
                    // Debug.Log($"PlatformBottom true");
                    return hit;
                } else {
                    // Debug.Log($"PlatformBottom false");
                    return new RaycastHit2D();
                }
            }
        }

        public RaycastHit2D PlatformBottomExtend
        {
            get 
            {   
                RaycastHit2D hit = Physics2D.Raycast(PlatformCheckBottom.position, Vector2.down, platformCheckBottomExtendDistance, whatIsGround);
                if (hit.collider != null && hit.collider.CompareTag(platformTag)) {
                    // Debug.Log($"PlatformBottom true");
                    return hit;
                } else {
                    // Debug.Log($"PlatformBottom false");
                    return new RaycastHit2D();
                }
            }
        }

        public RaycastHit2D PlatformTop
        {
            get 
            {   
                RaycastHit2D hit = Physics2D.Raycast(platformCheckTop.position, Vector2.up, platformCheckTopDistance, whatIsGround);
                if (hit.collider != null && hit.collider.CompareTag(platformTag)) {
                    Debug.Log($"PlatformBottom true");
                    return hit;
                } else {
                    Debug.Log($"PlatformBottom false");
                    return new RaycastHit2D();
                }
            }
        }

        public Collider2D PlatformOverlap
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


