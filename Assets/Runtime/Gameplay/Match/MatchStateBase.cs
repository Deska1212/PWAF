using UnityEngine;

namespace Core
{
	public abstract class MatchStateBase
	{
		public abstract void OnStateBegin(MatchManager match);
		public abstract void OnStateTick(MatchManager match, float dt);
		public abstract void OnStateEnd(MatchManager match);
	}
}