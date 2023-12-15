using System;
using Core;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI
{
	public class MainMenuManager : DeskaBehaviourSingleton<MainMenuManager>
	{
		#region FIELDS

		[SerializeField] private GameObject matchConfigUI;
		[SerializeField] private GameObject mainMenuUI;

		
		[SerializeField] private TextMeshProUGUI scoreToWinText;
		[SerializeField] private int scoreToWin;
		[SerializeField] private MatchType matchType;

		#endregion
		
		#region EVENTS

		public UnityEvent OnMainMenuInitialize;
		public UnityEvent<MatchData> OnEnterVersusMatch;
		public UnityEvent<MatchData> OnEnterBotMatch;
		public UnityEvent OnMainMenuDeinitialize;

		#endregion

		#region UNITY EVENT FUNCTIONS

		protected override void SingletonAwake()
		{
			base.SingletonAwake();
			OnMainMenuInitialize?.Invoke();
		}

		private void OnEnable()
		{
			
		}

		protected override void SingletonDestroy()
		{
			base.SingletonDestroy();
			OnMainMenuDeinitialize?.Invoke();
		}

		#endregion
		
		#region METHODS

		public void PressedVersus()
		{
			ToggleMainMenuUI(false);
			ToggleMatchConfigUI(true);
		}
		
		public void PressedInfinite()
		{
			
		}

		public void PressedWithABotMatch()
		{
			OnEnterBotMatch?.Invoke(GetMatchConfigSettings());
		}

		public void PressedWithAFriend()
		{
			// Assign our current config to GameManager
			GameManager.Instance.AssignMatchData(GetMatchConfigSettings());
			
			// Invoke event
			OnEnterVersusMatch?.Invoke(GetMatchConfigSettings());
			
			// Load the match scene
			// SceneLoader.Instance.LoadScene(SCENE.MATCH);
			SceneManager.LoadScene(2);
		}

		public void PressedIncrementScoreSetting()
		{
			if(scoreToWin >= 15) return;
			
			// Pass into match config struct
			scoreToWin++;
			Debug.Log("Pressed Inc Score");
			RefreshConfigUI();
		}

		public void PressedDecrementScoreSetting()
		{
			if(scoreToWin <= 3) return;
			
			// Pass into match config struct
			scoreToWin--;
			Debug.Log("Pressed Dec Score");
			RefreshConfigUI();
		}

		public void PressedBack()
		{
			ToggleMatchConfigUI(false);
			ToggleMainMenuUI(true);
		}

		public MatchData GetMatchConfigSettings()
		{
			MatchData config = new MatchData();
			config.scoreToWin = this.scoreToWin;
			config.matchType = this.matchType;
			return config;
		}

		#endregion
		
		#region PRIVATE FUNCTIONS

		private void ToggleMainMenuUI(bool toggle)
		{
			mainMenuUI.SetActive(toggle);
		}
		
		private void ToggleMatchConfigUI(bool toggle)
		{
			matchConfigUI.SetActive(toggle);
		}

		private void RefreshConfigUI()
		{
			scoreToWinText.text = scoreToWin.ToString();
		}

		
		

		#endregion
	}
}