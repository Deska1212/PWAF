using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.TextCore;

namespace Effects
{
	public class ParticleController : DeskaBehaviourSingleton<ParticleController>
	{
		[SerializeField] private GameObject ballWallContactFX;
		[SerializeField] private GameObject powerupWallContactFX;

		#region UNITY EVENT FUNCTIONS

		private void OnEnable()
		{
			MatchManager.Instance.GetBall().OnCollisionWithSidewall.AddListener(HandleBallCollisionWithWall);
		}

		private void OnDisable()
		{
			MatchManager.Instance.GetBall().OnCollisionWithSidewall.RemoveListener(HandleBallCollisionWithWall);
		}

		protected override void SingletonAwake()
		{
			base.SingletonAwake();
		}

		protected override void SingletonStart()
		{
			base.SingletonStart();
		}

		protected override void SingletonDestroy()
		{
			base.SingletonDestroy();
		}

		#endregion
		
		#region PUBLIC METHODS

		public void HandleBallCollisionWithWall(Vector2 position)
		{
			// Determine what sidewall the ball collided off based on the balls position when it collided

			if (position.x < 0)
			{
				// Left wall collision
				SpawnBallWallCollisionParticle(position, Facing.RIGHT);
				
			}
			else
			{
				// Right wall collision - Spawn particle facing left
				SpawnBallWallCollisionParticle(position, Facing.LEFT);				
			}
		}
		
		public void HandlePowerupCollisionWithWall(Vector2 position)
		{
			// Determine what sidewall the powerup collided off based on the powerups position when it collided

			if (position.x < 0)
			{
				// Left wall collision
				SpawnBallWallCollisionParticle(position, Facing.RIGHT);
				
			}
			else
			{
				// Right wall collision - Spawn particle facing left
				SpawnBallWallCollisionParticle(position, Facing.LEFT);				
			}
		}

		private void SpawnBallWallCollisionParticle(Vector2 position, Facing facing)
		{
			Vector2 rotation = facing == Facing.LEFT ? Vector2.left : Vector2.right;
			Quaternion rot = Quaternion.LookRotation(rotation, Vector3.forward);
			GameObject fx = Instantiate(ballWallContactFX, position, rot);
		}
		
		private void SpawnPowerupWallCollisionParticle(Vector2 position, Facing facing)
		{
			Vector2 rotation = facing == Facing.LEFT ? Vector2.left : Vector2.right;
			Quaternion rot = Quaternion.LookRotation(rotation, Vector3.forward);
			GameObject fx = Instantiate(powerupWallContactFX, position, rot);
		}

		#endregion

		

		private enum Facing
		{
			RIGHT,
			LEFT,
			UP,
			DOWN
		}
	}
}