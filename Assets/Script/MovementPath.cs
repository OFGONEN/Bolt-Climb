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
	public SharedReferenceNotifier notif_nut_transform;

// Private
	Transform nut_transform;
	float point_bottom;
	float point_gap;
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

		point_bottom = transform.position.y;
		point_gap = path_points[ path_points.Length - 1 ].y - point_bottom;
	}
#endregion

#region API
	public void OnNutTrigger()
	{
		nut_transform = notif_nut_transform.SharedValue as Transform;
		event_movement.Raise( path_index );
	}

	public float ReturnPathProgress()
	{
		return ( nut_transform.position.y - point_bottom ) / point_gap;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
	[ ShowInInspector ] float height = 11.3f;

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

	[ Button() ]
	void MultipyZones( int count )
	{
		var newPath = new List< Vector3 >();

		for( var i = 0; i < count; i++ )
		{
			for( var x = 0; x < path_points.Length; x++ )
			{
				newPath.Add( path_points[ x ] + Vector3.up * i * height );
			}
		}

		path_points = newPath.ToArray();
	}
#endif
#endregion
}