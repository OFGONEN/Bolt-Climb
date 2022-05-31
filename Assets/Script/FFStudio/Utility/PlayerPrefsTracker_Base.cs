/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public abstract class PlayerPrefsTracker_Base : ScriptableObject
    {
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
        public abstract void DeleteAll();
        public abstract void SetFloat( string key, float value );
		public abstract void SetInt( string key, int value );
		public abstract void SetString( string key, string value );
        [ OnInspectorInit() ]
        public abstract void Refresh();
#endregion

#region Implementation
#endregion
    }
}