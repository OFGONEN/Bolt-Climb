/* Created by and for usage of FF Studios (2021). */

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using System.Reflection;

namespace FFEditor
{
	public static class FFShortcutUtility
	{
		static private TransformData currentTransformData;
		static private string path_playerPrefsTracker = "Assets/Editor/tracker_playerPrefs.asset";

		[ MenuItem( "FFShortcut/TakeScreenShot #F12" ) ]
		public static void TakeScreenShot()
		{
			int counter = 0;
			var path = Path.Combine( Application.dataPath, "../", "ScreenShot_" + counter + ".png" );

			while( File.Exists( path ) ) // If file is not exits new screen shot will be a new file
			{
				counter++;
				path = Path.Combine( Application.dataPath, "../", "ScreenShot_" + counter + ".png" ); // ScreenShot_1.png
			}

			ScreenCapture.CaptureScreenshot( "ScreenShot_" + counter + ".png" );
			AssetDatabase.SaveAssets();

			Debug.Log( "ScreenShot Taken: " + "ScreenShot_" + counter + ".png" );
		}
		
		[ MenuItem( "FFShortcut/Select PlayerPrefsTracker _F8" ) ]
		static private void SelectPlayerPrefsTracker()
		{
			var tracker = AssetDatabase.LoadAssetAtPath( path_playerPrefsTracker, typeof( ScriptableObject ) );
			( tracker as PlayerPrefsTracker ).Refresh();
			Selection.SetActiveObjectWithContext( tracker, tracker );
		}

		[ MenuItem( "FFShortcut/Delete PlayerPrefs _F9" ) ]
		static private void ResetPlayerPrefs()
		{
			PlayerPrefsUtility.Instance.DeleteAll();
			Debug.Log( "PlayerPrefs Deleted" );
		}

		[ MenuItem( "FFShortcut/Previous Level _F10" ) ]
		static private void PreviousLevel()
		{
			var currentLevel = PlayerPrefs.GetInt( "Level" );

			currentLevel = Mathf.Max( currentLevel - 1, 1 );

			PlayerPrefs.SetInt( "Level", currentLevel );
			PlayerPrefs.SetInt( "Consecutive Level", currentLevel );

			Debug.Log( "Level Set:" + currentLevel );
		}

		[ MenuItem( "FFShortcut/Next Level _F11" ) ]
		static private void NextLevel()
		{
			var nextLevel = PlayerPrefs.GetInt( "Level" ) + 1;

			PlayerPrefs.SetInt( "Level", nextLevel );
			PlayerPrefs.SetInt( "Consecutive Level", nextLevel );

			Debug.Log( "Level Set:" + nextLevel );

		}

		[ MenuItem( "FFShortcut/Save All Assets _F12" ) ]
		static private void SaveAllAssets()
		{
			AssetDatabase.SaveAssets();
			Debug.Log( "AssetDatabase Saved" );
		}

		[ MenuItem( "FFShortcut/Select Game Settings &1" ) ]
		static private void SelectGameSettings()
		{
			var gameSettings = Resources.Load( "game_settings" );

			Selection.SetActiveObjectWithContext( gameSettings, gameSettings );
		}

		[ MenuItem( "FFShortcut/Select Level Data &2" ) ]
		static private void SelectLevelData()
		{
			var levelData = Resources.Load( "level_data_1" );

			Selection.SetActiveObjectWithContext( levelData, levelData );
		}

		[ MenuItem( "FFShortcut/Select App Scene &3" ) ]
		static private void SelectAppScene()
		{
			EditorSceneManager.OpenScene( "Assets/Scenes/app.unity" );
			var appScene = AssetDatabase.LoadAssetAtPath( "Assets/Scenes/app.unity", typeof( SceneAsset ) );

			Selection.SetActiveObjectWithContext( appScene, appScene );		
		}

		[ MenuItem( "FFShortcut/Select Play Mode Settings &4" ) ]
		static private void SelectPlayModeSettings()
		{
			var playModeSettings = AssetDatabase.LoadAssetAtPath( "Assets/Editor/PlayModeUtilitySettings.asset", typeof( ScriptableObject ) );

			Selection.SetActiveObjectWithContext( playModeSettings, playModeSettings );
		}

		[ MenuItem( "FFShortcut/Select Level Creator &5" ) ]
		static private void SelectLevelCreator()
		{
			var levelCreator = AssetDatabase.LoadAssetAtPath( "Assets/Editor/level_creator.asset", typeof( ScriptableObject ) );
			Selection.SetActiveObjectWithContext( levelCreator, levelCreator );
		}

		[ MenuItem( "FFShortcut/Select Level Environment Creator &6" ) ]
		static private void SelectLevelEnvironmentCreator()
		{
			var levelEnvironmentCreator = AssetDatabase.LoadAssetAtPath( "Assets/Editor/level_creator_environment.asset", typeof( ScriptableObject ) );
			Selection.SetActiveObjectWithContext( levelEnvironmentCreator, levelEnvironmentCreator );
		}

		[ MenuItem( "FFShortcut/Select Skin Library &7" ) ]
		static private void SelectSkinLibrary()
		{
			var skinLibrary = AssetDatabase.LoadAssetAtPath( "Assets/Scriptable_Object/Shared/library_skin.asset", typeof( ScriptableObject ) );
			Selection.SetActiveObjectWithContext( skinLibrary, skinLibrary );
		}

		[ MenuItem( "FFShortcut/Copy Global Transform &c" ) ]
		static private void CopyTransform()
		{
			currentTransformData = Selection.activeGameObject.transform.GetTransformData();
		}

		[ MenuItem( "FFShortcut/Paste Global Transform &v" ) ]
		static private void PasteTransform()
		{
			var gameObject = Selection.activeGameObject.transform;
			gameObject.SetTransformData( currentTransformData );
		}

		[ MenuItem( "FFShortcut/Kill All Tweens %#t" ) ]
		private static void KillAllTweens()
		{
			DOTween.KillAll();
			FFLogger.Log( "[FF] DOTween: Kill All" );
		}

		[ MenuItem( "FFShortcut/Clear Console %#x" ) ]
		private static void ClearLog()
		{
			var assembly = Assembly.GetAssembly( typeof( UnityEditor.Editor ) );
			var type = assembly.GetType( "UnityEditor.LogEntries" );
			var method = type.GetMethod( "Clear" );
			method.Invoke( new object(), null );
		}
	}
}