using System;
using System.Collections.Generic;
using Runtime.Data;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Core
{
	/// <summary>
	/// This class is responsible for managing spawning, de-spawning, activation,
	/// and deactivation on powerups
	/// </summary>
	public class PowerupManager : DeskaBehaviourSingleton<PowerupManager>
	{

		[SerializeField] private List<PowerupEffect> activeEffects = new List<PowerupEffect>();
		[SerializeField] public List<GameObject> fieldedPowerups = new List<GameObject>();
		[SerializeField] private float angleOffset; 
		
		[SerializeField] private Transform[] spawns;
		[SerializeField] private Powerups powerupsList;

		[SerializeField] private float timeBetweenPowerupSpawns;
		private float spawnTimer;


		public UnityEvent<Powerup> OnSpawnPowerup;

		#region UNITY EVENT FUNCS

		private void Update()
		{
			if (MatchManager.Instance.MatchOngoing)
			{
				spawnTimer += Time.deltaTime;
			}

			for (int i = 0; i < fieldedPowerups.Count; ++i)
			{
				if (fieldedPowerups[i] == null)
				{
					fieldedPowerups.RemoveAt(i);
				}
			}


			for (int i = 0; i < activeEffects.Count; ++i)
			{
				
				if (activeEffects[i].flaggedForDestroy)
				{
					activeEffects.RemoveAt(i);
					continue; // Continue and don't tick an item we've just destroyed, null ref
				}
				
				activeEffects[i].TickPowerupEffect(Time.deltaTime);
			}

			if (spawnTimer >= timeBetweenPowerupSpawns)
			{
				SpawnPowerup();
			}
			
			

		}
		protected override void SingletonAwake()
		{
			base.SingletonAwake();
		}

		protected override void SingletonStart()
		{
			base.SingletonStart();
			BindRoundResetEvents();
		}

		

		protected override void SingletonDestroy()
		{
			base.SingletonDestroy();
			UnbindRoundResetEvent();
		}

		#endregion

		#region PUBLIC METHODS

		public void ActivatePowerup(Powerup powerup)
		{
			switch (powerup.data.type)
			{
				case PowerupType.DEFLECT:
					InitDeflectEffect();
					Debug.Log("Activating Powerup " + PowerupType.DEFLECT);
					break;
				case PowerupType.PADDLE_SIZE_PLUS:
					InitPaddleSizePlusEffect();
					Debug.Log("Activating Powerup " + PowerupType.PADDLE_SIZE_PLUS);
					break;
				case PowerupType.CENTER_LOCKOUT:
					InitCenterLockEffect();
					Debug.Log("Activating Powerup " + PowerupType.CENTER_LOCKOUT);
					break;
				case PowerupType.PADDLE_DRIVE:
					InitPaddleDriveEffect();
					Debug.Log("Activating Powerup " + PowerupType.PADDLE_DRIVE);
					break;
				case PowerupType.HOMING_BALL:
					InitHomingBallEffect();
					Debug.Log("Activating Powerup " + PowerupType.HOMING_BALL);
					break;
				
			}
		}

		public void SpawnPowerup()
		{
			spawnTimer = 0;
			// Figure out which powerup to spawn and instantiate it
			int randPowerupIdx = Random.Range(0, powerupsList.powerups.Count);
			GameObject objToSpawn = powerupsList.powerups[randPowerupIdx];
			Vector2 spawnPos = spawns[Random.Range(0, spawns.Length)].position;
			GameObject powerupGO = Instantiate(objToSpawn, spawnPos, transform.rotation);
			Rigidbody2D powerupRb = powerupGO.GetComponent<Rigidbody2D>();

			// Find direction to field origin from spawned location
			Vector2 velocityDir = Vector2.zero - (Vector2)powerupGO.transform.position;
			velocityDir.Normalize();
			
			// Rotate that vector slightly by random angle between range
			Quaternion rot = Quaternion.Euler(0, 0, Random.Range(-angleOffset, angleOffset));
			Vector2 offsetVelocityDir = rot * velocityDir;

			// Scale normalised, rotated vector and launch powerup
			Vector2 scaledVelocityDir = offsetVelocityDir * Constants.POWERUP_KICKOFF_VELOCITY;
			powerupRb.velocity = scaledVelocityDir;
			powerupRb.angularVelocity = Random.Range(-Constants.POWERUP_KICKOFF_ANGULAR_VEL_MAX, Constants.POWERUP_KICKOFF_ANGULAR_VEL_MAX);
			
			
			
			
			// Add powerup to active in field list
			fieldedPowerups.Add(powerupGO);
			
			OnSpawnPowerup.Invoke(powerupGO.GetComponent<Powerup>());
		}


		#endregion

		#region PRIVATE FUNCTIONS

		private void RemoveAllFieldedPowerups()
		{
			for (int i = 0; i < fieldedPowerups.Count; ++i)
			{
				Destroy(fieldedPowerups[i]);
			}

			fieldedPowerups.Clear();

		}

		private void RemoveAllActiveEffects()
		{
			foreach (PowerupEffect effect in activeEffects)
			{
				effect.StopPowerupEffect();
				effect.flaggedForDestroy = true;
			}
		}

		private void BindRoundResetEvents()
		{
			MatchManager.Instance.OnRoundReset.AddListener(RemoveAllFieldedPowerups);
			MatchManager.Instance.OnRoundReset.AddListener(RemoveAllActiveEffects);
		}

		private void UnbindRoundResetEvent()
		{
			MatchManager.Instance.OnRoundReset.RemoveListener(RemoveAllFieldedPowerups);
			MatchManager.Instance.OnRoundReset.RemoveListener(RemoveAllActiveEffects);
		}

		#endregion

		#region EFFECT INIT FUNCTIONS
		
		private void InitDeflectEffect()
		{
			DeflectPowerupEffect effect = new DeflectPowerupEffect();
			activeEffects.Add(effect);
		}

		private void InitPaddleSizePlusEffect()
		{
			// Affect the size of the paddle that last hit the ball
			PaddleController p = MatchManager.Instance.GetBall().GetPaddleLastHit();
			PaddleSizePlusPowerupEffect effect = new PaddleSizePlusPowerupEffect(p);
			activeEffects.Add(effect);
		}

		private void InitCenterLockEffect()
		{
			// If there is already a center lockout effect active, collecting a new one stops it
			foreach (PowerupEffect powerupEffect in activeEffects)
			{
				if (powerupEffect is CenterLockPowerupEffect)
				{
					powerupEffect.StopPowerupEffect();
					return;
				}
			}
			
			CenterLockPowerupEffect effect = new CenterLockPowerupEffect();
			activeEffects.Add(effect);
		}

		private void InitPaddleDriveEffect()
		{
			PaddleController p = MatchManager.Instance.GetBall().GetPaddleLastHit();
			PaddleDrivePowerupEffect effect = new PaddleDrivePowerupEffect(p);
			activeEffects.Add(effect);
		}

		private void InitHomingBallEffect()
		{
			PaddleController p = MatchManager.Instance.GetBall().GetPaddleLastHit();
			HomingBallPowerupEffect effect = new HomingBallPowerupEffect(p);
			activeEffects.Add(effect);
		}

		#endregion
		

	}
}