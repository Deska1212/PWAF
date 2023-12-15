using System;
using UnityEngine;
using TMPro;
using Core;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI
{
	/// <summary>
	/// This class that manages the in-game UI elements, such as the score, and other on-screen information
	/// </summary>
	public class GameUIController : DeskaBehaviourSingleton<GameUIController>
	{
		
		#region EVENTS

		public UnityEvent OnPauseButtonTouched;
		
		#endregion
		
		#region PRIVATE FIELDS

		[SerializeField] private TextMeshProUGUI redScoreUI;
		[SerializeField] private TextMeshProUGUI blueScoreUI;

		[SerializeField] private GameObject matchConcludedPanel;
		[SerializeField] private TextMeshProUGUI matchWinnerText;

		[SerializeField] private GameObject pausedMatchPanel;
		[SerializeField] private TextMeshProUGUI pauseText;

		
		#endregion
		
		#region UNITY EVENT FUNCTIONS

		protected override void SingletonStart()
		{
			base.SingletonStart();
			
			// Event subscriptions
			MatchManager.Instance.OnScoreChanged.AddListener(RefreshScoreUI);
			MatchManager.Instance.OnMatchConcluded.AddListener(InitialiseMatchConcludedPanel);
			MatchManager.Instance.OnRematch.AddListener(DeactivateMatchConcludedPanel);
			
			MatchManager.Instance.OnMatchTogglePause.AddListener(RefreshPausedUI);
		}

		protected override void SingletonDestroy()
		{
			base.SingletonDestroy();
			
			// Event un-subscriptions
			MatchManager.Instance.OnScoreChanged.RemoveListener(RefreshScoreUI);
			MatchManager.Instance.OnMatchConcluded.RemoveListener(InitialiseMatchConcludedPanel);
			MatchManager.Instance.OnRematch.RemoveListener(DeactivateMatchConcludedPanel);
			
			MatchManager.Instance.OnMatchTogglePause.RemoveListener(RefreshPausedUI);
		}

		#endregion

		#region PRIVATE METHODS

		private void RefreshScoreUI(int blueScore, int redScore)
		{
			blueScoreUI.text = blueScore.ToString();
			redScoreUI.text = redScore.ToString();
		}

		private void RefreshPausedUI()
		{
			// TODO: This is temporarily text, eventually it will be a sprite swap
			string text = MatchManager.Instance.MatchPaused ? "play" : "pause";
			pauseText.text = text;
			pausedMatchPanel.SetActive(MatchManager.Instance.MatchPaused);
		}

		private void InitialiseMatchConcludedPanel()
		{
			matchConcludedPanel.SetActive(true);
			matchWinnerText.text = $"{MatchManager.Instance.ScoredLastGoal} wins!";
		}

		private void DeactivateMatchConcludedPanel()
		{
			matchConcludedPanel.SetActive(false);
		}

		#endregion

		#region PUBLIC METHODS

		public void ReturnToMenu()
		{
			SceneManager.LoadScene(1);
		}

		public void Rematch()
		{
			MatchManager.Instance.Rematch();
		}

		public void PauseButtonTouched()
		{
			OnPauseButtonTouched?.Invoke();
		}

		#endregion
	}
}