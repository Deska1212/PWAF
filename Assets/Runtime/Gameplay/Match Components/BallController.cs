using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Core
{
	/// <summary>
	/// Controls the ball's movement, handles collision events with paddles, walls, powerups, and goal areas
	/// </summary>
	public class BallController : DeskaBehaviour
	{
		#region EVENTS
		
		[HideInInspector] public UnityEvent<PaddleController> OnCollisionWithPaddle;
		[HideInInspector] public UnityEvent<Vector2> OnCollisionWithSidewall;
		[HideInInspector] public UnityEvent<Vector2> OnCollisionWithCenterLockout;
		[HideInInspector] public UnityEvent<MatchManager.Side> OnEnterGoalArea;
		[HideInInspector] public UnityEvent<MatchManager.Side> OnEnterFieldSide;
		
		
		
		#endregion

		#region PRIVATE FIELDS

		private Rigidbody2D rb;
		
		[SerializeField] private bool sideWallRecentCollision;
		[SerializeField] private float timeSinceLastSidewallCollision;

		[SerializeField] private bool paddleRecentCollision;
		[SerializeField] private float timeSinceLastPaddleCollision;

		[SerializeField] private GameObject ballSpriteGO;
		[SerializeField] private Color redPaddleColour;
		[SerializeField] private Color bluePaddleColour;
		
		[SerializeField] private PaddleController paddleLastHit;
		[SerializeField] private TrailRenderer trailRenderer;
		private MatchManager.Side currentBallSide;
		
		[SerializeField] private PaddleController paddleToHomeTo;
		[SerializeField] private LineRenderer lineRenderer;
		private bool isHoming;
		


		#endregion

		#region PROPERTIES

		public MatchManager.Side CurrentBallSide
		{
			get
			{
				return currentBallSide;
			}
			set
			{
				// If nothing changed do nothing
				if (value == currentBallSide)
				{
					return;
				}
				else
				{
					currentBallSide = value;
					OnEnterFieldSide.Invoke(currentBallSide);
				}	
			}

		}

		public PaddleController PaddleToHomeTo
		{
			get
			{
				return paddleToHomeTo;
			}

			set
			{
				if (paddleToHomeTo != null)
				{
					// We are already homing to a paddle, end that first
					OnEndHoming();
				}

				if (value == paddleToHomeTo)
				{
					return; // Same value, do nothing
				}
				
				paddleToHomeTo = value;

				if (paddleToHomeTo != null)
				{
					OnStartHoming();
				}
				else
				{
					OnEndHoming();
				}
			}
		}

		#endregion

		#region UNITY EVENT METHODS

		private void OnEnable()
		{
			// Listen to kickoff event on match manager
			MatchManager.Instance.OnRoundKickoff?.AddListener(BallKickoff);
			MatchManager.Instance.OnRoundReset?.AddListener(ResetBallToStart);
			MatchManager.Instance.OnRoundKickoff?.AddListener(EnableTrail);
			MatchManager.Instance.OnRoundEnded?.AddListener(DisableTrail);
		}

		

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			Vector2 worldToViewportPos = Camera.main.WorldToViewportPoint(transform.position);
			
			CheckForSidewallCollision(worldToViewportPos);

			if (MatchManager.Instance.MatchOngoing)
			{
				CheckForGoalCollision(worldToViewportPos);

			}

			UpdateRecentPaddleCollisionTimer();
			CurrentBallSide = GetBallSideOnField();
			
			if (isHoming)
			{
				OnHomingTick();
			}
		}

		


		private void OnTriggerEnter2D(Collider2D col)
		{
			Debug.Log(col.tag);
			if (col.TryGetComponent(out PaddleController paddle) && !paddleRecentCollision)
			{
				HandlePaddleCollision(paddle);
			}

			if (col.gameObject.tag == "CenterLockout")
			{
				HandleCenterLockoutCollision();
			}
			
			// For Infinite Match
			if (col.gameObject.CompareTag("RedPaddle") && MatchManager.Instance.IsInfiniteMatch) ;
			{
				HandleInfiniteMatchPaddleCollision();
			}
		}
		
		#endregion
		
		

		

		

		

		

		#region PUBLIC METHODS

		

		public void BallKickoff()
		{
			// float yVel = GetKickoffVelocity() * DeskaUtils.RandomSign();
			// float xVel = Constants.BALL_VELOCITY_RATIO * yVel;

			float xVel = 0;
			float yVel = DeskaUtils.RandomSign() * Constants.BALL_BASE_VELOCITY;

			Vector2 kickoffVelocity = new Vector2(xVel, yVel);
			rb.velocity = kickoffVelocity;
		}

		public void InvertYVelocity()
		{
			Vector2 vel = rb.velocity;
			vel.y = -vel.y;
			rb.velocity = vel;
		}
		
		public void InvertXVelocity()
		{
			Vector2 vel = rb.velocity;
			vel.x = -vel.x;
			rb.velocity = vel;
		}

		#endregion

		#region HOMING FUNCTIONALITY

		private void OnStartHoming()
		{
			isHoming = true;
			Color col = PaddleToHomeTo.GetPaddleSide() == MatchManager.Side.BLUE ? bluePaddleColour : redPaddleColour;
				
			LeanTween.color(ballSpriteGO, col, Constants.HOMING_COLOUR_FLASH_TIME)
				.setLoopPingPong((int)(Constants.HOMING_BALL_EFFECT_TIME / (Constants.HOMING_COLOUR_FLASH_TIME * 2)))
				.setOnComplete(ResetBallColour);
			
			EnableHomingLineRenderer(col);
			
		}
		
		private void OnHomingTick()
		{
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, PaddleToHomeTo.transform.position);
			
			if (PaddleToHomeTo == paddleLastHit)
				return;

			Vector2 dir = PaddleToHomeTo.transform.position - transform.position;


			float dist = Mathf.Abs(transform.position.x - PaddleToHomeTo.transform.position.x);


			float accel = dir.x * Constants.HOMING_BALL_ACCEL_FACTOR;
			float damping = (1 / dist) * Constants.HOMING_BALL_DAMP_FACTOR; // Damping gets higher as distance gets lower, end result is modified by factor

			float xForce = accel / damping;
			xForce = Mathf.Clamp(xForce, -Constants.HOMING_BALL_MAX_HOMING_FORCE, Constants.HOMING_BALL_MAX_HOMING_FORCE);

			Vector2 force = new Vector2(xForce, 0f);
			rb.velocity += force * Time.deltaTime;
			
			
		}

		private void OnEndHoming()
		{
			isHoming = false;
			LeanTween.cancel(ballSpriteGO);
			DisableHomingLineRenderer();
		}


		private void EnableHomingLineRenderer(Color col)
		{
			lineRenderer.positionCount = 2;
			lineRenderer.startColor = col;
			lineRenderer.endColor = col;
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, PaddleToHomeTo.transform.position);
		}

		private void DisableHomingLineRenderer()
		{
			lineRenderer.positionCount = 0;
		}

		#endregion



		#region COLLISION HANDLING

		private void CheckForSidewallCollision(Vector2 worldToViewportPos)
		{
			UpdateRecentSidewallCollisionTimer();

			// Convert world to viewport point for side wall check - we have a very short cooldown for side wall
			// check to prevent weird behaviour where the ball gets stuck
			
			
			// Check side walls with a slight margin
			// TODO: Use a const value for margin
			
			// Check collisions with side walls
			bool wallCheck = (worldToViewportPos.x <= Constants.SIDE_WALL_MARGIN_X || worldToViewportPos.x >= 1f - Constants.SIDE_WALL_MARGIN_X) && !sideWallRecentCollision;
				
			if (wallCheck)
			{
				// Handle wall collision
				// TODO: Conversion from world to viewport getting the radius of the ball from constants and factoring that instead of a set margin
				HandleWallCollision();
			}
		}
		
		

		private void CheckForGoalCollision(Vector2 worldToViewportPos)
		{
			// Check collisions with goal areas - done for both - param side is side that ball entered goal on
			if (worldToViewportPos.y <= Constants.GOAL_MARGIN)
			{
				// Ball has hit the top of the screen (blues goal area)
				OnEnterGoalArea.Invoke(MatchManager.Side.BLUE);
			}

			if (worldToViewportPos.y >= 1f - Constants.GOAL_MARGIN)
			{
				// Ball has hit the top of the screen (reds goal area)
				OnEnterGoalArea.Invoke(MatchManager.Side.RED);
			}


			// TODO: Goal checks go here, note that the margin is slightly larger than the side wall margins
		}

		private void HandleWallCollision()
		{
			HandleRecentCollision();
			InvertXVelocity();
			OnCollisionWithSidewall.Invoke(transform.position);
		}
		
		private void HandleCenterLockoutCollision()
		{
			HandleRecentCollision();
			InvertYVelocity();
			OnCollisionWithCenterLockout.Invoke(transform.position);
		}
		
		private void HandleRecentCollision()
		{
			timeSinceLastSidewallCollision = 0f;
			sideWallRecentCollision = true;
		}
		
		


		private void HandlePaddleCollision(PaddleController paddle)
		{
			paddleRecentCollision = true;
			timeSinceLastPaddleCollision = 0f;
			
			// Caclulate relative intersect
			float relativeIntersect = transform.position.x - paddle.transform.position.x;
			float normalizedRelativeIntersect = relativeIntersect / (Constants.BASE_PADDLE_WIDTH / 2f);
			float bounceXVelocity = normalizedRelativeIntersect * (Constants.BOUNCE_MAX_X_VELOCITY_AS_PERCENTAGE * Constants.BALL_BASE_VELOCITY);
			
			Vector2 newVelocity = new Vector2(bounceXVelocity, Mathf.Clamp(-rb.velocity.y, -1, 1) * Constants.BALL_BASE_VELOCITY);
			newVelocity.y = newVelocity.y * paddle.GetPaddleForceModifier();
			rb.velocity = newVelocity;

			paddleLastHit = paddle;
			OnCollisionWithPaddle.Invoke(paddle);
		}

		private void HandleInfiniteMatchPaddleCollision()
		{
			paddleRecentCollision = true;
			timeSinceLastPaddleCollision = 0f;
			
			InvertYVelocity();
		}



		private void UpdateRecentSidewallCollisionTimer()		  
		{
			timeSinceLastSidewallCollision += Time.deltaTime;

			if (timeSinceLastSidewallCollision > Constants.SIDE_WALL_RECENT_COLLISION_COOLDOWN_TIME)
			{
				sideWallRecentCollision = false;
			}
		}
		
		private void UpdateRecentPaddleCollisionTimer()		  
		{
			timeSinceLastPaddleCollision += Time.deltaTime;

			if (timeSinceLastPaddleCollision > Constants.PADDLE_RECENT_COLLISION_COOLDOWN_TIME)
			{
				paddleRecentCollision = false;
			}
		}

		#endregion

		

		#region HELPERS
		
		public PaddleController GetPaddleLastHit()
		{
			return paddleLastHit;
		}

		private void EnableTrail()
		{
			trailRenderer.enabled = true;
		}
		
		private void DisableTrail()
		{
			trailRenderer.enabled = false;
		}
		
		private void ResetBallColour()
		{
			Color col = Color.white;
			ballSpriteGO.GetComponent<SpriteRenderer>().color = col;
		}
		
		private void ResetBallToStart()
		{
			transform.position = Vector2.zero;
			rb.velocity = Vector2.zero;
			HandleRecentCollision();
		}
		
		private MatchManager.Side GetBallSideOnField()
		{
			if (transform.position.y == 0f)
			{
				return MatchManager.Side.NONE;
			}
			else if(transform.position.y > 0f)
			{
				return MatchManager.Side.RED;
			}
			else
			{
				return MatchManager.Side.BLUE;
			}
		}

		#endregion
	}
}