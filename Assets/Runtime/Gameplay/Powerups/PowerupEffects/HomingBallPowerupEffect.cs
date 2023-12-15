namespace Core
{
	public class HomingBallPowerupEffect : PowerupEffect
	{
		private PaddleController assignedPaddle;

		public HomingBallPowerupEffect(PaddleController p)
		{
			assignedPaddle = p;
			StartPowerupEffect();
		}

		public override void StartPowerupEffect()
		{
			maxActiveTime = Constants.HOMING_BALL_EFFECT_TIME;
			timer = maxActiveTime;
			
			MatchManager.Instance.GetBall().PaddleToHomeTo = assignedPaddle;
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
			// Clear the paddle to home to value on ball, this stops all homing functionality
			MatchManager.Instance.GetBall().PaddleToHomeTo = null;
			flaggedForDestroy = true;
		}
	}
}