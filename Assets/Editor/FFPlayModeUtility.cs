/* Created by and for usage of FF Studios (2021). */

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FFEditor
{
    [ InitializeOnLoad ]
	public static class FFPlayModeUtility
	{
		static private FFPlayModeUtilitySettings playModeUtilitySettings;
		static private bool initialized = false;

		private static FFPlayModeUtilitySettings PlayModeUtilitySettings
		{
			get
			{
				if( playModeUtilitySettings == null )
				{
					//Find PlayModeUtilitySettings file 
					var path_playModeUtilitySettings = "Assets/Editor/PlayModeUtilitySettings.asset";
					    playModeUtilitySettings      = AssetDatabase.LoadAssetAtPath( path_playModeUtilitySettings,
																	 typeof( FFPlayModeUtilitySettings ) ) as FFPlayModeUtilitySettings;
				}

				return playModeUtilitySettings;
			}
		}

		static FFPlayModeUtility()
		{
			if( !initialized )
			{
				EditorApplication.playModeStateChanged += PlayModeChange;
				initialized = true;
			}

			//Find PlayModeUtilitySettings file 
			var path_playModeUtilitySettings = "Assets/Editor/PlayModeUtilitySettings.asset";
			    playModeUtilitySettings      = AssetDatabase.LoadAssetAtPath( path_playModeUtilitySettings, 
                                                                     typeof( FFPlayModeUtilitySettings ) ) as FFPlayModeUtilitySettings;

			if( playModeUtilitySettings == null )
			{
				Debug.LogError( "PlayModeUtilitySettings is not found" );
			}
		}

		static void PlayModeChange( PlayModeStateChange change )
		{
			switch( change )
			{
				case PlayModeStateChange.ExitingEditMode:
					if( PlayModeUtilitySettings.useDefaultScene )
					{
						if( EditorSceneManager.GetActiveScene().buildIndex != PlayModeUtilitySettings.defaultSceneIndex )
						{
							PlayModeUtilitySettings.lastSceneIndex = EditorSceneManager.GetActiveScene().path;
							EditorSceneManager.OpenScene( SceneUtility.GetScenePathByBuildIndex( PlayModeUtilitySettings.defaultSceneIndex ), OpenSceneMode.Single );
						}
					}
					break;
				case PlayModeStateChange.EnteredEditMode:
					if( PlayModeUtilitySettings.useDefaultScene )
					{
						EditorSceneManager.OpenScene( PlayModeUtilitySettings.lastSceneIndex, OpenSceneMode.Single );
					}
					break;

				default:
					return;
			}
		}
	}
}