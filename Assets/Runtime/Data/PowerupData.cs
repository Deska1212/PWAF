using UnityEngine;

namespace Core
{
	[CreateAssetMenu(fileName = "PowerupData", menuName = "Powerups/PowerupData", order = 0)]
	public class PowerupData : ScriptableObject
	{
		public PowerupType type;
	}
}