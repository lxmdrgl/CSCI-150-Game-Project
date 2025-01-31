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

        [SerializeField] private Vector2 hitBoxPosition;
        [SerializeField] private Vector2 hitBoxSize;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private string platformTag;
        [SerializeField] private Transform ledgeCheckVertical;


        public bool Ground 
        {
            get => Physics2D.OverlapBox(GroundCheck.position, groundCheckSize, 0f, whatIsGround);
		}

        public bool Platform 
        {
            get
            {
                Collider2D collider = Physics2D.OverlapBox(GroundCheck.position, groundCheckSize, 0f, whatIsGround);
                return collider != null && collider.CompareTag(platformTag);
            }
        }

        public Collider2D PlatformCollider 
        {
            get {
                Collider2D collider = Physics2D.OverlapBox(GroundCheck.position, groundCheckSize, 0f, whatIsGround);

                if (collider != null && collider.CompareTag(platformTag))
                {
                    return collider;
                } else {
                    return null;
                }
            }
        }

        public bool PlatformOverlap {
            get
            {
                Collider2D collider = Physics2D.OverlapBox(hitBoxPosition, hitBoxSize, 0f, whatIsGround);
                return collider != null && collider.CompareTag(platformTag);
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
    }
}


