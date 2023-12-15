using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Data
{
	[CreateAssetMenu(fileName = "Powerups", menuName = "Powerups/Powerups", order = 0)]
	public class Powerups : ScriptableObject
	{
		// Holds all powerup prefab references
		[SerializeField] public List<GameObject> powerups;
	}
}