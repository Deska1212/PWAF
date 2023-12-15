using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;

namespace Core
{
	/// <summary>
	/// MatchStateRoundEnded starts once a goal has been scored by either player, it is responsible for checking
	/// if the match end conditions have been met, if they haven't it is responsible for conducting
	/// the round ended cooldown, then reseting the game, and moving back to the starting cooldown... If the
	/// match end conditions have been met, it is responsible for moving to MatchStateConcluded after a short timer
	///
	/// Note: A round is defined as from the start of the starting countdown, to the end of the round ended countdown
	/// </summary>
	public class MatchStateRoundEnded : MatchStateBase
	{
		public override void OnStateBegin(MatchManager match)
		{
			// End of match - Start end of round countdown
			match.EndMatchSequence();
		}

		public override void OnStateTick(MatchManager match, float dt)
		{
			match.EndCountdown -= dt;

			if (match.EndCountdown <= 0f)
			{
				bool winConditionMet = match.BlueScore == match.GetMatchData().scoreToWin || match.RedScore == match.GetMatchData().scoreToWin;

				// Check if we have met the Match end condition (# of games is done)
				if (winConditionMet)
				{
					// Match has completed all games, thus match is concluded
					match.ChangeMatchState(new MatchStateConcluded());
				}
				else
				{
					// Reset match at the end of the cooldown
					match.MatchRoundReset();
					match.ChangeMatchState(new MatchStateCountdown());
				}
			}
		}

		public override void OnStateEnd(MatchManager match)
		{
			
		}
	}
}