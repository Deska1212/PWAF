using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
	[System.Serializable]
	public class DeflectPowerupEffect : PowerupEffect
	{
		public DeflectPowerupEffect()
		{
			flaggedForDestroy = false;
			StartPowerupEffect();
		}

		public override void StartPowerupEffect()
		{
			if (!flaggedForDestroy)
			{
				MatchManager.Instance.GetBall().InvertYVelocity();
			}
			
			StopPowerupEffect();
		}

		public override void TickPowerupEffect(float dt)
		{
			
		}

		public override void StopPowerupEffect()
		{
			flaggedForDestroy = true;
		}
	}
}