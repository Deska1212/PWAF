using Unity.VisualScripting;

namespace Core
{
	class PaddleDrivePowerupEffect : PowerupEffect
	{
		private PaddleController assignedPaddle;

		public PaddleDrivePowerupEffect(PaddleController p)
		{
			assignedPaddle = p;
			StartPowerupEffect();
		}

		public override void StartPowerupEffect()
		{
			maxActiveTime = Constants.PADDLE_DRIVE_EFFECT_TIME;
			timer = maxActiveTime;

			assignedPaddle.TogglePaddleDriveEffect(true);
		}

		public override void TickPowerupEffect(float dt)
		{
			timer -= dt;

			if (timer <= 0f)
			{
				StopPowerupEffect();
			}
		}

		public override void StopPowerupEffect()
		{
			assignedPaddle.TogglePaddleDriveEffect(false);
			flaggedForDestroy = true;
		}
	}
}