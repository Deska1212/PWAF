using System;
using UnityEngine;

namespace Core
{
	public class GameManager : DeskaBehaviourSingleton<GameManager>
	{
		[SerializeField] private MatchData currentMatchData;

		

		public void AssignMatchData(MatchData data)
		{
			currentMatchData = data;
		}
		
		public MatchData GetMatchData()
		{
			return currentMatchData;
		}
	}
}