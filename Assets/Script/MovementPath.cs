/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;
using DG.Tweening;

public class MovementPath : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
	public int path_index;
    public MovementPath_Set path_set;
    public Vector3[] path_points;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnEnable()
	{
		path_set.AddDictionary( path_index, path_points );
	}

	private void OnDisable()
	{
		path_set.RemoveDictionary( path_index );
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
	[ Button() ]
	void ImportPath()
	{
		path_points = GetComponent< DOTweenPath >().wps.ToArray();
	}
#endif
#endregion
}
