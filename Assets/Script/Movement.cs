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
	[ SerializeField ] MovementPath_Set path_Set;
	[ SerializeField ] Velocity velocity;
	[ SerializeField ] SharedFloat shared_velocity_pathSpeed;
	[ SerializeField ] GameEvent event_nut_path_update;

  [ Title( "Setup" ) ]
	[ SerializeField ] Transform transform_movement;
	[ SerializeField ] Transform transform_rotate;

	// Private
	Tween pathTween;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnDisable()
	{
		pathTween.KillProper();
	}
#endregion

#region API
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
		pathTween = transform_movement.DOPath( pathPoints, Mathf.Max( GameSettings.Instance.movement_pathSpeed_minumum, velocity.CurrentVelocity * shared_velocity_pathSpeed.sharedValue ), PathType.Linear )
		.SetLookAt( 0 )
		.SetSpeedBased()
		// .SetRelative()
		.OnUpdate( OnPathUpdate )
		.SetEase( Ease.Linear )
		.OnComplete( onPathComplete );
	}

	public void DoPathEnd( int index, TweenCallback onPathComplete )
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
		pathTween = transform_movement.DOPath( pathPoints, Mathf.Max( GameSettings.Instance.movement_pathSpeed_minumum, velocity.CurrentVelocity * shared_velocity_pathSpeed.sharedValue ), PathType.Linear )
		.SetLookAt( 0 )
		.SetSpeedBased()
		// .SetRelative()
		.OnUpdate( DoRotate )
		.SetEase( Ease.Linear )
		.OnComplete( onPathComplete );
	}

 // Return true if this is on the fall down point
	public bool OnMovement( float minPosition )
	{
		var position = transform_movement.position;
		position   += Vector3.up * velocity.CurrentVelocity * Time.deltaTime;
		position.y  = Mathf.Max( minPosition, position.y );

		transform_movement.position = position;

		DoRotate();

		return Mathf.Approximately( position.y, minPosition );
	}

	public void OnMovement()
	{
		var position = transform_movement.position;
		position   += Vector3.up * velocity.CurrentVelocity * Time.deltaTime;

		transform_movement.position = position;
		DoRotate();
	}
#endregion

#region Implementation
	void OnPathUpdate()
	{
		DoRotate();
		event_nut_path_update.Raise();
	}

	void DoRotate()
	{
		transform_rotate.Rotate( Vector3.up * velocity.CurrentVelocity * Time.deltaTime * GameSettings.Instance.movement_rotation_cofactor , Space.Self );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}