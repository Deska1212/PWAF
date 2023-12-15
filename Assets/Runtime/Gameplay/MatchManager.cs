using System;
using Core;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Core
{
	

	public class MatchManager : DeskaBehaviourSingleton<MatchManager>
	{

		#region EVENTS

		[HideInInspector] public UnityEvent<int, int> OnScoreChanged; // Top and bottom score
		[HideInInspector] public UnityEvent OnMatchTogglePause;

		// Match events
		[HideInInspector] public UnityEvent OnMatchInitialise; // Invoked only once when the match FIRST gets initialised
		[HideInInspector] public UnityEvent OnStartMatchCountdown; // Invoked when the match countdown starts
		[HideInInspector] public UnityEvent OnRoundKickoff; // Invoked when the match start countdown ends
		[HideInInspector] public UnityEvent<Side> OnMatchGoalScored; // Invoked when the ball enters a goal area
		[HideInInspector] public UnityEvent OnRoundEnded; // Invoked when the match end countdown is started
		[HideInInspector] public UnityEvent OnRoundReset; // Invoked when the match end countdown is finished
		[HideInInspector] public UnityEvent OnMatchConcluded; // Invoked when the match's conclusion condition is met
		[HideInInspector] public UnityEvent OnRematch; // Invoked when the match is restarted



		#endregion

		#region PRIVATE FIELDS

		[SerializeField] private int redScore;
		[SerializeField] private int blueScore;
		[SerializeField] private bool isInfiniteMatch;
		private bool matchOngoing;
		private bool matchPaused;

		private float startCountdown;
		private float endCountdown;


		private MatchStateBase matchState;

		[SerializeField] private MatchData matchData;

		private Side scoredLastGoal;

		#endregion

		#region PROPERTIES

		public int RedScore
		{
			get { return redScore; }
			set
			{
				redScore = value;
				OnScoreChanged?.Invoke(blueScore, redScore);
			}
		}

		public int BlueScore
		{
			get { return blueScore; }
			set
			{
				blueScore = value;
				OnScoreChanged?.Invoke(blueScore, redScore);
			}
		}

		public bool MatchOngoing
		{
			get => matchOngoing;
			private set => matchOngoing = value;
		}

		public float StartCountdown
		{
			get => startCountdown;
			set => startCountdown = value;
		}

		public float EndCountdown
		{
			get => endCountdown;
			set => endCountdown = value;
		}

		public Side ScoredLastGoal
		{
			get => scoredLastGoal;
			set => scoredLastGoal = value;
		}

		public bool MatchPaused
		{
			get => matchPaused;
			private set => matchPaused = value;
		}

		public bool IsInfiniteMatch
		{
			get => isInfiniteMatch;
			private set => isInfiniteMatch = value;
		}

		#endregion

		#region UNITY EVENT FUNCTIONS

		protected override void SingletonAwake()
		{
			base.SingletonAwake();
		}

		protected override void SingletonStart()
		{
			GetBall().OnEnterGoalArea.AddListener(MatchGoalScored);
			GameUIController.Instance.OnPauseButtonTouched?.AddListener(PauseButtonTouched);
			
			MatchInitialise();
		}
		
		protected override void SingletonDestroy()
		{
			base.SingletonDestroy();
		}

		private void Update()
		{
			if (matchState != null)
			{
				matchState.OnStateTick(this, Time.deltaTime);
			}
			else
			{
				Debug.Log("Could not find match state!");
			}
		}

		#endregion

		#region PRIVATE METHODS

		private void ResetScores()
		{
			RedScore = 0;
			BlueScore = 0;

		}

		#endregion

		#region PUBLIC METHODS

		public void MatchInitialise()
		{
			Debug.Log("Match Init");
			ResetScores();
			InitialiseMatchData();
			ChangeMatchState(new MatchStateCountdown());
			OnMatchInitialise?.Invoke();
		}

		public void StartMatchCountdown()
		{
			StartCountdown = Constants.MATCH_START_COOLDOWN;

			// Start the match cooldown, 
			OnStartMatchCountdown?.Invoke();
		}

		public void MatchKickoff()
		{
			MatchOngoing = true;
			OnRoundKickoff?.Invoke();
		}

		public void MatchGoalScored(Side side)
		{
			// Switch on side and add opposite side score
			switch (side)
			{
				case Side.RED:
					BlueScore++;
					scoredLastGoal = Side.BLUE;
					break;
				case Side.BLUE:
					RedScore++;
					scoredLastGoal = Side.RED;
					break;
			}

			Logger.Log($"Ball has collided with {side} goal", DeskaLogger.LogLevel.Verbose);

			MatchOngoing = false;

			ChangeMatchState(new MatchStateRoundEnded());
			OnMatchGoalScored?.Invoke(side);
		}

		public void EndMatchSequence()
		{
			// Start match end countdown
			EndCountdown = Constants.MATCH_END_COOLDOWN;

			OnRoundEnded?.Invoke();
		}

		public void MatchRoundReset()
		{
			// Reset paddle and ball positions - ball should reset itself
			// Destroy any powerups on the field - powerups should destroy themselves
			// Remove any powerup effects

			OnRoundReset?.Invoke();
		}

		public void MatchConcluded()
		{
			OnMatchConcluded?.Invoke();
		}

		public void ChangeMatchState(MatchStateBase newState)
		{
			if (newState == null)
			{
				Logger.Log($"Could not find match state!", DeskaLogger.LogLevel.Error);
			}

			Logger.Log($"Changing match state from {matchState} to {newState}", DeskaLogger.LogLevel.Verbose);

			if (matchState != null)
			{
				matchState.OnStateEnd(this);
			}

			matchState = newState;
			matchState.OnStateBegin(this);

		}

		public void Rematch()
		{
			MatchRoundReset();
			MatchInitialise();
			ChangeMatchState(new MatchStateCountdown());
			OnRematch?.Invoke();
		}

		public void Deinitialize()
		{
			matchData = Constants.DEFAULT_MATCH_DATA;
		}


		#endregion

		#region HELPERS

		public PaddleController GetRedPaddle()
		{
			PaddleController redPaddle;
			bool foundPaddle = GameObject.FindWithTag("RedPaddle").TryGetComponent(out redPaddle);

			if (foundPaddle)
			{
				return redPaddle;
			}
			else
			{
				Debug.Log("GetPaddle helper could not find a PaddleController - have you checked paddles are tagged?");
			}
			
			return null;
		}

		public PaddleController GetBluePaddle()
		{
			PaddleController bluePaddle;
			bool foundPaddle = GameObject.FindWithTag("RedPaddle").TryGetComponent(out bluePaddle);

			if (foundPaddle)
			{
				return bluePaddle;
			}
			else
			{
				Debug.Log("GetPaddle helper could not find a PaddleController - have you checked paddles are tagged?");
			}

			
			return null;
		}

		public BallController GetBall()
		{
			// Returns a reference to the ball controller
			// TODO: Validate and check if we can find one
			BallController ball = GameObject.FindObjectOfType<BallController>();
			if (ball != null)
			{
				return ball;
			}
			else
			{
				Logger.Log("Could not find ball controller!", DeskaLogger.LogLevel.Critical);
			}
			
			return null;
		}
		
		public MatchData GetMatchData()
		{
			return matchData;
		}

		#endregion

		#region PRIVATES

		private void InitialiseMatchData()
		{
			if (GameObject.FindObjectOfType<GameManager>() == null)
			{
				Debug.Log("GAME MANAGER WAS NOT FOUND... USING DEFAULT MATCH DATA");
				matchData = Constants.DEFAULT_MATCH_DATA;
			}
			else
			{
				Debug.Log("GAME MANAGER FOUND... INITIALISING MATCH DATA");
				matchData = GameManager.Instance.GetMatchData();
			}
		}

		private void PauseButtonTouched()
		{
			MatchPaused = !MatchPaused;
			Time.timeScale = MatchPaused ? 0f : 1f;	
			OnMatchTogglePause?.Invoke();
		}

		#endregion

		public enum Side
		{
			BLUE,
			RED,
			NONE
		}
	}
}