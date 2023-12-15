using System;
using UnityEngine;

namespace Core
{
	public class PowerupParticleController : MonoBehaviour
	{
		[SerializeField] private GameObject wallContactFX;
		private Powerup powerup;
		
		

		#region UNITY EVENT FUNCTIONS

		private void OnEnable()
		{
			powerup.OnCollisionWithSideWall.AddListener(HandleCollisionWithWall);
		}

		private void OnDisable()
		{
			powerup.OnCollisionWithSideWall.RemoveListener(HandleCollisionWithWall);
		}

		private void Awake()
		{
			powerup = GetComponent<Powerup>();
		}
		
		#endregion
		
		#region PRIVATE FUNCTIONS

		private void HandleCollisionWithWall(Vector2 position)
		{
			if (transform.position.x < 0)
			{
				// Left wall collision
				SpawnWallCollisionFX(position, false);
				
			}
			else
			{
				// Right wall collision - Spawn particle facing left
				SpawnWallCollisionFX(position, true);				
			}
		}

		private void SpawnWallCollisionFX(Vector2 position, bool facingLeft)
		{
			Vector2 rotation = facingLeft ? Vector2.left : Vector2.right;
			Quaternion rot = Quaternion.LookRotation(rotation, Vector3.forward);
			GameObject fx = Instantiate(wallContactFX, position, rot);
		}

		#endregion
	}
}