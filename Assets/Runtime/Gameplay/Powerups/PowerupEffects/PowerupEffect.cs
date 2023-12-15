using UnityEngine;

namespace Core
{
	[System.Serializable]
	public abstract class PowerupEffect
	{
		protected float maxActiveTime;
		public float timer;
		public PaddleController affectedController;
		public bool flaggedForDestroy;

		public abstract void StartPowerupEffect();

		public abstract void TickPowerupEffect(float dt);

		public abstract void StopPowerupEffect();
		
	}
}