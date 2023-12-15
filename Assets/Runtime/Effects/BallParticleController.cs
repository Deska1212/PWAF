using System;
using Core;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Effects
{
	public class BallParticleController : MonoBehaviour
	{
		[SerializeField] private GameObject wallContactFX;
		[SerializeField] private GameObject paddleContactFXBlue;
		[SerializeField] private GameObject paddleContactFXRed;
		private BallController ballController;
		
		

		#region UNITY EVENT FUNCTIONS

		private void OnEnable()
		{
			ballController.OnCollisionWithSidewall.AddListener(HandleCollisionWithWall);
			ballController.OnCollisionWithPaddle.AddListener(HandleCollisionWithPaddle);
		}

		private void OnDisable()
		{
			ballController.OnCollisionWithSidewall.RemoveListener(HandleCollisionWithWall);
			ballController.OnCollisionWithPaddle.RemoveListener(HandleCollisionWithPaddle);
		}

		private void Awake()
		{
			ballController = GetComponent<BallController>();
		}

		#endregion
		
		#region PRIVATE FUNCTIONS

		private void HandleCollisionWithWall(Vector2 position)
		{
			// Determine what sidewall the ball collided off based on the balls position when it collided

			if (transform.position.x < 0)
			{
				// Left wall collision
				SpawnWallCollisionFX(position, Facing.RIGHT);
				
			}
			else
			{
				// Right wall collision - Spawn particle facing left
				SpawnWallCollisionFX(position, Facing.LEFT);				
			}
		}

		private void SpawnWallCollisionFX(Vector2 position, Facing facing)
		{
			Vector2 rotation = facing == Facing.LEFT ? Vector2.left : Vector2.right;
			Quaternion rot = Quaternion.LookRotation(rotation, Vector3.forward);
			GameObject fx = Instantiate(wallContactFX, position, rot);
		}

		private void HandleCollisionWithPaddle(PaddleController paddleController)
		{
			// Check which side we hit, e.g. if we hit top we would rotate the particle to face downward & spawn the coresponding colour
			switch (paddleController.GetPaddleSide())
			{
				case MatchManager.Side.RED:
					Instantiate(paddleContactFXRed, transform.position, quaternion.identity);	
					break;
				case MatchManager.Side.BLUE:
					Instantiate(paddleContactFXBlue, transform.position, quaternion.identity);	
					break;
			}
			
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