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
#endregion

#region API
	public void DoPath( Vector3[] pathPoints, TweenCallback onPathComplete )
	{
		transform_movement.DOPath( pathPoints, velocity.CurrentVelocity, PathType.Linear )
		.SetLookAt( 0, -Vector3.up )
		.SetSpeedBased()
		.SetRelative()
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