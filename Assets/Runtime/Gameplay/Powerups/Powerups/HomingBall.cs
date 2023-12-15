using UnityEngine;

namespace Core
{
	public class HomingBall : Powerup
	{
		public override void AcquirePowerup()
		{
			PaddleController p = MatchManager.Instance.GetBall().GetPaddleLastHit();
			float zRot = p.GetPaddleSide() == MatchManager.Side.RED ? 180f : 0f; // If red paddle acquired this powerup we must rotate so faces into field
			Instantiate(activationEffect, p.transform.position, Quaternion.Euler(0, 0, zRot));
			
			PowerupManager.Instance.ActivatePowerup(this);
		}
	}
}