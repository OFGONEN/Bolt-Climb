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
	public IntGameEvent event_movement;
	public int path_index;
    public MovementPath_Set path_set;
    public Vector3[] path_points; // Local points
#endregion

#region Properties
#endregion

#region Unity API
	private void OnDisable()
	{
		path_set.RemoveDictionary( path_index );
	}

	private void Start()
	{
		path_set.AddDictionary( path_index, path_points );
	}
#endregion

#region API
	public void OnNutTrigger()
	{
		event_movement.Raise( path_index );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
	[ Button() ]
	public void RotatePoints()
	{
		for( var i = 0; i < path_points.Length; i++ )
		{
			path_points[ i ] = Quaternion.AngleAxis( transform.eulerAngles.y, Vector3.up ) * path_points[ i ];
		}
	}

	public void MovePoints()
	{
		for( var i = 0; i < path_points.Length; i++ )
		{
			path_points[ i ] += transform.position;
		}
	}

	[ Button() ]
	void ImportPath()
	{
		path_points = new Vector3[ transform.childCount ];

		for( var i = 0; i < transform.childCount; i++ )
		{
			path_points[ i ] = transform.GetChild( i ).localPosition;
		}
	}

	[ Button() ]
	void ZeroOutDepth()
	{
		for( var i = 0; i < path_points.Length; i++ )
		{
			path_points[ i ] = path_points[ i ].SetZ( 0 );
		}
	}
#endif
#endregion
}
