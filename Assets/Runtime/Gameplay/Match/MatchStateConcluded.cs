using UnityEngine;

namespace Core
{
	/// <summary>
	/// MatchStateConcluded starts once the match finished condition has been met (Number of rounds have been played)
	/// and is responsible for bringing up the match concluded UI
	/// </summary>
	public class MatchStateConcluded : MatchStateBase
	{
		public override void OnStateBegin(MatchManager match)
		{
			// Show Concluded UI
			match.MatchConcluded();
		}

		public override void OnStateTick(MatchManager match, float dt)
		{
			
		}

		public override void OnStateEnd(MatchManager match)
		{
			
		}
	}
}