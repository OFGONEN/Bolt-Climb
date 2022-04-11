/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEditor;
using Sirenix.OdinInspector;

public class Bolt : MonoBehaviour
{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    [ ShowInInspector, BoxGroup( "EditorOnly" ) ] private GameObject bolt_prefab;
    [ ShowInInspector, BoxGroup( "EditorOnly" ) ] private int bolt_count;
    [ ShowInInspector, BoxGroup( "EditorOnly" ) ] private float bolt_height;

    [ Button() ]
    public void PlaceBolts()
    {
        for( var i = 0; i < bolt_count; i++ )
        {
		    var prefab = PrefabUtility.InstantiatePrefab( bolt_prefab, transform ) as GameObject;
			var position = transform.position;
			position.y += bolt_height * i;
			prefab.transform.position = position;
		}
	}
#endif
#endregion
}
