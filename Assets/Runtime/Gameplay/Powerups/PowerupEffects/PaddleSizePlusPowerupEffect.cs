using UnityEngine;

namespace Core
{
	public class PaddleSizePlusPowerupEffect : PowerupEffect
	{
		private PaddleController paddleToEffect;
		private float originalPaddleWidth;
		
		public PaddleSizePlusPowerupEffect(PaddleController p)
		{
			paddleToEffect = p;
			StartPowerupEffect();
		}

		public override void StartPowerupEffect()
		{
			maxActiveTime = Constants.PADDLE_SIZE_PLUS_EFFECT_TIME;
			timer = maxActiveTime;
			
			paddleToEffect.TogglePlusPaddleSizeEffect(true);
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
			paddleToEffect.TogglePlusPaddleSizeEffect(false);
			flaggedForDestroy = true;
		}
	}
}