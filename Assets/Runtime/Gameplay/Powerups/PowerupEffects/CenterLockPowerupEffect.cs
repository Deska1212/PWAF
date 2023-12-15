using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Core
{
	public class CenterLockPowerupEffect : PowerupEffect
	{
		private CenterLockoutController centerLockoutController;

		public CenterLockPowerupEffect()
		{
			StartPowerupEffect();
		}

		public override void StartPowerupEffect()
		{
			centerLockoutController = GameObject.FindObjectOfType<CenterLockoutController>();
			maxActiveTime = Constants.CENTER_LOCKOUT_EFFECT_TIME;
			timer = maxActiveTime;
			
			ActivateCenterLockout();
		}

		public override void TickPowerupEffect(float dt)
		{

			timer -= dt;

			if (timer <= 0)
			{
				StopPowerupEffect();
			}
		}

		public override void StopPowerupEffect()
		{
			centerLockoutController.DisableCenterLockout();
			flaggedForDestroy = true;
		}
		
		
		private void ActivateCenterLockout()
		{
			centerLockoutController.EnableCenterLockout();
		}
	}
}