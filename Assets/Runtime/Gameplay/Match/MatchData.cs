using System;
using UnityEngine;

namespace Core
{
	[Serializable]
	public struct MatchData
	{
		public int scoreToWin;
		public MatchType matchType;
		public MatchDifficulty matchDifficulty;

		public MatchData(int score, MatchType type, MatchDifficulty difficulty)
		{
			scoreToWin = score;
			matchType = type;
			matchDifficulty = difficulty;
		}
	}

	public enum MatchType
	{
		VERSUS,
		VERSUS_BOT
	}

	public enum MatchDifficulty
	{
		EASY,
		MEDIUM,
		HARD
	}
}