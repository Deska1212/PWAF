using System.Text.RegularExpressions;
using UnityEngine;

namespace Core
{
	/// <summary>
	/// MatchStateOngoing is responsible for closing the round off after a goal and moving to
	/// MatchStateRoundEnded
	/// </summary>
	public class MatchStateOngoing : MatchStateBase
	{
		public override void OnStateBegin(MatchManager match)
		{
			// Kickoff
			match.MatchKickoff();
		}

		public override void OnStateTick(MatchManager match, float dt)
		{
			
		}

		public override void OnStateEnd(MatchManager match)
		{
			

		}
		
	}
}