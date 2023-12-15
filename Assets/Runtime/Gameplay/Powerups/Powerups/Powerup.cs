using System.Resources;
using Effects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ParticleSystemJobs;
using Random = UnityEngine.Random;


namespace Core
{
	public abstract class Powerup : MonoBehaviour, IPowerup
	{
		// Private/Protected Fields
		private float timeSinceLastSidewallCollision;
		private bool sidewallRecentCollision;
		private Rigidbody2D rb;
		protected int maxBounces;
		protected int bounces = 0;
		protected bool isActive = false; // Activates when powerup first passes into the playing field
		
		// Properties/Publics
		public PowerupData data;
		[HideInInspector] public UnityEvent<Vector2> OnCollisionWithSideWall;
		[HideInInspector] public UnityEvent<GameObject> OnPowerupAquired;

		[SerializeField] protected GameObject activationEffect;



		#region UNITY EVENT FUNCTIONS
		
		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
			maxBounces = Random.Range(Constants.POWERUP_MIN_BOUNCES, Constants.POWERUP_MAX_BOUNCES); // Generate a bounces before destroy value
		}

		private void Update()
		{
			// This powerups position on the viewport
			Vector2 worldToViewportPos = Camera.main.WorldToViewportPoint(transform.position);

			// We only need to check for sidewall collision once the powerup has entered the field from spawning
			if (!isActive)
			{
				CheckInitialFielding(worldToViewportPos);
			}
			else
			{
				CheckSidewallCollision(worldToViewportPos);
			}

		}



		#endregion
		

		#region PUBLIC INTERFACE

		public abstract void AcquirePowerup();

		public virtual void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.CompareTag("Ball"))
			{
				AcquirePowerup();
				Destroy(this.gameObject);
			}
		}

		#endregion
		

		#region Private Functions

		private void CheckInitialFielding(Vector2 worldToViewportPos)
		{
			// Bool checks if powerup is in game field 
			bool isFielded = worldToViewportPos.x > Constants.SIDE_WALL_MARGIN_X && worldToViewportPos.x < 1f - Constants.SIDE_WALL_MARGIN_X;
			if (isFielded)
			{
				isActive = true;
			}
		}
		
		private void CheckSidewallCollision(Vector2 worldToViewportPos)
		{
			UpdateRecentCollisionTimer();
			
			// Check to see if we've hit the side of the viewport 
			bool isCollidedWithWall = worldToViewportPos.x <= Constants.SIDE_WALL_MARGIN_X || worldToViewportPos.x >= 1f - Constants.SIDE_WALL_MARGIN_X;
			
			// If we have hit the side of the viewport, havent collided recently and are active then its a legit collision
			bool canCollide = isCollidedWithWall && !sidewallRecentCollision && isActive;
				
			// Handle that legit collision
			if (canCollide)
			{
				// TODO: Conversion from world to viewport getting the radius of the powerup from constants and factoring that instead of a set margin from powerup origin
				HandleWallCollision();
			}
		}

		private void HandleWallCollision()
		{
			// Flag that we've collided with a wall - prevents weird jittery movement with double collisions 
			HandleRecentCollision();
			FlipXVelocity();
			OnCollisionWithSideWall?.Invoke(transform.position);

			// Check if we've hit our max bounce limit and destroy if so
			bounces++;
			if (bounces > maxBounces)
			{
				PowerupManager.Instance.fieldedPowerups.Remove(this.gameObject);
				Destroy(this.gameObject);
			}
		}
		
		private void HandleRecentCollision()
		{
			timeSinceLastSidewallCollision = 0f;
			sidewallRecentCollision = true;
		}
		
		private void FlipXVelocity()
		{
			Vector2 vel = rb.velocity;
			Vector2 newVelocity = new Vector2(-vel.x, vel.y);
			rb.velocity = newVelocity;
		}

		private void UpdateRecentCollisionTimer()
		{
			timeSinceLastSidewallCollision += Time.deltaTime;

			// If we've passed the time since wall collision threshold, we can collide again knowing its not a weird double collision
			if (timeSinceLastSidewallCollision > Constants.SIDE_WALL_RECENT_COLLISION_COOLDOWN_TIME)
			{
				sidewallRecentCollision = false;
			}
		}

		#endregion
	}

	public enum PowerupType
	{
		DEFLECT,
		PADDLE_SIZE_PLUS,
		CENTER_LOCKOUT,
		PADDLE_DRIVE,
		HOMING_BALL
	}


}
