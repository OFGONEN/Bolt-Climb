/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif

namespace FFStudio
{
    [ CreateAssetMenu( fileName = "tracker_playerPrefs", menuName = "FF/Utility/PlayerPrefs Tracker (Editor)" ) ]
    public class PlayerPrefsTracker : PlayerPrefsTracker_Base
    {
#if UNITY_EDITOR

#region Fields
        [ SerializeField ] List< PlayerPrefs_Int    > integers = new List< PlayerPrefs_Int    >();
        [ SerializeField ] List< PlayerPrefs_Float  > floats   = new List< PlayerPrefs_Float  >();
        [ SerializeField ] List< PlayerPrefs_String > strings  = new List< PlayerPrefs_String >();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
        public override void DeleteAll()
        {
			floats.Clear();
			integers.Clear();
			strings.Clear();

			EditorUtility.SetDirty( this );
		}

		public override void SetInt( string key, int value )
		{
			for( var i = 0; i < integers.Count; i++ )
			{
				if( integers[ i ].key == key )
				{
					var prefs = integers[ i ];
					prefs.value = value;
					integers[ i ] = prefs;
					return;
				}
			}

			var newPrefs = new PlayerPrefs_Int { key = key, value = value };
			integers.Add( newPrefs );

			EditorUtility.SetDirty( this );
		}
		
        public override void SetFloat( string key, float value )
        {
            for( var i = 0; i < floats.Count; i++ )
            {
				if( floats[ i ].key == key )
				{
					var prefs = floats[ i ];
					prefs.value = value;
					floats[ i ] = prefs;
					return;
				}
			}

			var newPrefs = new PlayerPrefs_Float { key = key, value = value };
			floats.Add( newPrefs );

			EditorUtility.SetDirty( this );
        }

		public override void SetString( string key, string value )
		{
			for( var i = 0; i < strings.Count; i++ )
			{
				if( strings[ i ].key == key )
				{
					var prefs = strings[ i ];
					prefs.value = value;
					strings[ i ] = prefs;
					return;
				}
			}

			var newPrefs = new PlayerPrefs_String { key = key, value = value };
			strings.Add( newPrefs );

			EditorUtility.SetDirty( this );
		}
        
        [ OnInspectorInit() ]
        public override void Refresh()
        {
            if( integers != null )
                for( var i = 0; i < integers.Count; i++ )
                    integers[ i ].Refresh();

            if( floats != null )
                for( var i = 0; i < floats.Count; i++ )
                    floats[ i ].Refresh();

            if( strings != null )
                for( var i = 0; i < strings.Count; i++ )
                    strings[ i ].Refresh();


			EditorUtility.SetDirty( this );
		}
#endregion

#region Implementation
#endregion

#endif
    }
}