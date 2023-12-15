using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
	public class SceneLoader : DeskaBehaviourSingleton<SceneLoader>
	{
		private readonly Dictionary<SCENE, int> scenes = new Dictionary<SCENE, int>()
		{
			{ SCENE.PERSISTANTS, 0},
			{ SCENE.MENU, 1},
			{ SCENE.MATCH, 2}
	
		};

		public void LoadScene(SCENE sceneToLoad)
		{
			int idx = 0;
			bool valid = scenes.TryGetValue(sceneToLoad, out idx);

			if (valid)
			{
				SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
				SceneManager.LoadScene(idx);
			}
			else
			{
				Logger.Log("Error: Cound not load scene", DeskaLogger.LogLevel.Critical);
			}
		}
	}

	public enum SCENE
	{
		PERSISTANTS,
		MENU,
		MATCH
	}
}