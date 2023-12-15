using UnityEngine;

namespace Core
{
	/// <summary>
	/// MatchStateCountdown starts at the beginning of a match, is responsible for counting down
	/// the start of the match and moving to MatchStateOngoing after kickoff
	/// </summary>
	public class MatchStateCountdown : MatchStateBase
	{
		public override void OnStateBegin(MatchManager match)
		{
			// Start match countdown
			match.StartMatchCountdown(); 
			
			// Make sure time scale is reset at the start of the match
			Time.timeScale = 1f;
		}

		public override void OnStateTick(MatchManager match, float dt)
		{
			// Tick match cooldown
			match.StartCountdown -= dt;
			
			// Check if start cooldown is 0
			if (match.StartCountdown <= 0f)
			{
				match.ChangeMatchState(new MatchStateOngoing());
			}
		}

		public override void OnStateEnd(MatchManager match)
		{
			
		}
	}
}