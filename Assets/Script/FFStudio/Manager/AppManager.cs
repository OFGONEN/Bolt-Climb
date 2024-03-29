/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FFStudio
{
	public class AppManager : MonoBehaviour
	{
#region Fields
	[ Title( "Event Listeners" ) ]
		public EventListenerDelegateResponse loadNewLevelListener;
		public EventListenerDelegateResponse resetLevelListener;

	[ Title( "Fired Events" ) ]
		public GameEvent levelLoaded;
		public SharedFloatNotifier levelProgress;
#endregion

#region Unity API
		private void OnEnable()
		{
			loadNewLevelListener.OnEnable();
			resetLevelListener.OnEnable();
		}
		
		private void OnDisable()
		{
			loadNewLevelListener.OnDisable();
			resetLevelListener.OnDisable();
		}
		
		private void Awake()
		{
			Application.targetFrameRate = 60;


			loadNewLevelListener.response = LoadNewLevel;
			resetLevelListener.response   = ResetLevel;
		}

		private void Start()
		{
			var eventSystem = GameObject.Find( "EventSystem" );

			if( eventSystem != null )
			{
				FFLogger.Log( "Another EventSystem is disabled", eventSystem );
				eventSystem.SetActive( false );
			}

			StartCoroutine( LoadLevel() );
		}
#endregion

#region API
#endregion

#region Implementation
		private void ResetLevel()
		{
			var operation = SceneManager.UnloadSceneAsync( CurrentLevelData.Instance.levelData.scene_index );
			operation.completed += ( AsyncOperation operation ) => StartCoroutine( LoadLevel() );
		}
		
		private IEnumerator LoadLevel()
		{
			CurrentLevelData.Instance.currentLevel_Real = PlayerPrefsUtility.Instance.GetInt( "Level", 1 );
			CurrentLevelData.Instance.currentLevel_Shown = PlayerPrefsUtility.Instance.GetInt( "Consecutive Level", 1 );

			CurrentLevelData.Instance.LoadCurrentLevelData();

			var levelData = CurrentLevelData.Instance.levelData;
			
            // Reset incrementals
            if( levelData.incremental_set && CurrentLevelData.Instance.currentLevel_Shown <= CurrentLevelData.Instance.currentLevel_Real )
            {
				PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.velocity_index, levelData.incremental_set_index_velocity  );
				PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.durability_index, levelData.incremental_set_index_durability );
				PlayerPrefsUtility.Instance.SetInt( levelData.name, 1 );
			}

			// SceneManager.LoadScene( CurrentLevelData.Instance.levelData.sceneIndex, LoadSceneMode.Additive );
			var operation = SceneManager.LoadSceneAsync( CurrentLevelData.Instance.levelData.scene_index, LoadSceneMode.Additive );

			levelProgress.SharedValue = 0;

			while( !operation.isDone )
			{
				yield return null;

				levelProgress.SharedValue = operation.progress;
			}

			levelLoaded.Raise();
		}
		
		private void LoadNewLevel()
		{
			CurrentLevelData.Instance.currentLevel_Real++;
			CurrentLevelData.Instance.currentLevel_Shown++;
			PlayerPrefsUtility.Instance.SetInt( "Level", CurrentLevelData.Instance.currentLevel_Real );
			PlayerPrefsUtility.Instance.SetInt( "Consecutive Level", CurrentLevelData.Instance.currentLevel_Shown );

			var operation = SceneManager.UnloadSceneAsync( CurrentLevelData.Instance.levelData.scene_index );
			operation.completed += ( AsyncOperation operation ) => StartCoroutine( LoadLevel() );
		}
#endregion
	}
}