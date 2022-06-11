/* Created by and for usage of FF Studios (2021). */

using UnityEditor;
using UnityEngine;

namespace FFStudio
{
    [ CreateAssetMenu( fileName = "utility_playerPrefs", menuName = "FF/Utility/PlayerPrefs" ) ]
	public class PlayerPrefsUtility : ScriptableObject
    {
#region Fields
#if UNITY_EDITOR
		PlayerPrefsTracker_Base PlayerPrefsTracker => ( PlayerPrefsTracker_Base )AssetDatabase.LoadAssetAtPath( "Assets/Editor/tracker_playerPrefs.asset",
																											    typeof( PlayerPrefsTracker_Base ) );
#endif
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
        public void DeleteAll()
        {
#if UNITY_EDITOR
			PlayerPrefsTracker.DeleteAll();
#endif
			PlayerPrefs.DeleteAll();
		}

		public void SetInt( string key, int value )
		{
#if UNITY_EDITOR
			PlayerPrefsTracker.SetInt( key, value );
#endif
			PlayerPrefs.SetInt( key, value );
		}

		public void AddInt( string key, int value )
		{
			var baseValue = PlayerPrefs.GetInt( key, 0 );
#if UNITY_EDITOR
			PlayerPrefsTracker.SetInt( key, baseValue + value );
#endif
			PlayerPrefs.SetInt( key, baseValue + value );
		}

		public int GetInt( string key, int defaultValue )
		{
			return PlayerPrefs.GetInt( key, defaultValue );
		}

        public void SetFloat( string key, float value )
        {
#if UNITY_EDITOR
			PlayerPrefsTracker.SetFloat( key, value );
#endif
			PlayerPrefs.SetFloat( key, value );
		}

		public float GetFloat( string key, float defaultValue )
		{
			return PlayerPrefs.GetFloat( key, defaultValue );
		}

		public void SetString( string key, string value )
		{
#if UNITY_EDITOR
			PlayerPrefsTracker.SetString( key, value );
#endif
			PlayerPrefs.SetString( key, value );
		}

		public string GetString( string key, string defaultValue )
		{
			return PlayerPrefs.GetString( key, defaultValue );
		}
#endregion

#region Fields (Singleton Related)
        private static PlayerPrefsUtility instance;

        private delegate PlayerPrefsUtility ReturnPlayerPrefsUtility();
        private static ReturnPlayerPrefsUtility returnInstance = LoadInstance;

		public static PlayerPrefsUtility Instance => returnInstance();
#endregion

#region Implementation
        private static PlayerPrefsUtility LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< PlayerPrefsUtility >( "utility_playerPrefs" );

			returnInstance = ReturnInstance;

			return instance;
		}

		private static PlayerPrefsUtility ReturnInstance()
        {
            return instance;
        }
#endregion
    }
}