using System;
using Unity.Mathematics;
using UnityEngine;

namespace Core
{
	public class CenterLock : Powerup
	{
		public override void AcquirePowerup()
		{
			Instantiate(activationEffect, Vector2.zero, quaternion.identity);
			
			PowerupManager.Instance.ActivatePowerup(this);
		}
	}
}