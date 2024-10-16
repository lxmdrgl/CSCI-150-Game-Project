using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Vector2 groundCheckSize;
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private LayerMask whatIsGround;

        public bool Ground 
        {
            get => Physics2D.OverlapBox(GroundCheck.position, groundCheckSize, 0f, whatIsGround);
		}

        public bool WallFront 
        {
			get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsGround);
		}

        public bool WallBack 
        {
			get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsGround);
		}
    }
}


