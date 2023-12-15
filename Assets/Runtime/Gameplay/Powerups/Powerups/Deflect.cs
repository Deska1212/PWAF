using System;
using Unity.Mathematics;
using UnityEngine;

namespace Core
{
	public class Deflect : Powerup
	{

		public override void AcquirePowerup()
		{
			PowerupManager.Instance.ActivatePowerup(this);
			Vector2 ballPos = MatchManager.Instance.GetBall().transform.position;
			Instantiate(activationEffect, ballPos, quaternion.identity);
		}
	}
}