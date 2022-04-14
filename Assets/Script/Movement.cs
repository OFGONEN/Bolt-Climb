/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Movement : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
	[ SerializeField ] private MovementPath_Set path_Set;
	[ SerializeField ] private Velocity velocity;

  [ Title( "Setup" ) ]
	[ SerializeField ] private Transform transform_movement;
	[ SerializeField ] private Transform transform_rotate;
#endregion

#region Properties
#endregion

#region Unity API
	public void DoPath( int index, TweenCallback onPathComplete )
	{
		Vector3[] pathPoints;
		path_Set.itemDictionary.TryGetValue( index, out pathPoints );

#if UNITY_EDITOR
		if( pathPoints == null )
		{
			FFLogger.LogError( $"Path Index {index} is NULL" );
			return;
		}
#endif

		transform_movement.DOPath( pathPoints, velocity.CurrentSpeed, PathType.Linear )
		.SetLookAt( 0, -Vector3.up )
		.SetSpeedBased()
		.OnUpdate( DoRotate )
		.OnComplete( onPathComplete );
	}
#endregion

#region API
#endregion

#region Implementation
	void OnMovement( float minPosition )
	{
		var position = transform_movement.position;
		position   += Vector3.up * velocity.CurrentSpeed * Time.deltaTime;
		position.y  = Mathf.Max( minPosition, position.y );

		transform_movement.position = position;
	}

	void DoRotate()
	{
		transform_rotate.Rotate( Vector3.up * velocity.CurrentSpeed * Time.deltaTime * GameSettings.Instance.movement_rotation_cofactor , Space.Self );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}