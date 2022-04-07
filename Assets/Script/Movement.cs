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
	public MovementPath_Set path_Set;
	public IncrementalMovement incremental_movement;

  [ Title( "Setup" ) ]
	public Transform rotate_transform;
	// Private Fields
	[ ShowInInspector, ReadOnly ] IncrementalMovementData movement_data;
	[ ShowInInspector, ReadOnly ] float speed_current;
	float falldown_position;

	// Delegates
	UnityMessage updateMethod;

    // Properties
    public float CurrentSpeed => speed_current;
#endregion

#region Properties
    private void Awake()
    {
		updateMethod = ExtensionMethods.EmptyMethod;

		movement_data = incremental_movement.ReturnIncremental( PlayerPrefs.GetInt( "movement", 0 ) );
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region Unity API
    public void StartAcceleration( float fallDownPosition )
    {
		falldown_position = fallDownPosition;
		speed_current     = Mathf.Max( 0, speed_current );
		updateMethod      = OnAcceleration;
	}

    public void StopAcceleration()
    {
		updateMethod  = OnGravity;
	}

	public void DoIdle()
	{
		speed_current = 0;
		updateMethod = ExtensionMethods.EmptyMethod;
	}

	[ Button() ]
	public void DoPath( int index )
	{
		updateMethod = DoRotate;

		Vector3[] pathPoints;

		path_Set.itemDictionary.TryGetValue( index, out pathPoints );
#if UNITY_EDITOR
		if( pathPoints == null )
		{
			FFLogger.LogError( $"Path Index {index} is NULL" );
			DoIdle();
		}
#endif
		transform.DOPath( pathPoints, speed_current, PathType.Linear )
		.SetLookAt( 0, -Vector3.up )
		.SetSpeedBased()
		.OnComplete( OnPathComplete );
	}
#endregion

#region API
#endregion

#region Implementation
    void OnAcceleration()
    {
		speed_current = Mathf.Min( 
			movement_data.incremental_speed_max, 
			speed_current + Time.deltaTime * movement_data.incremental_speed_max / movement_data.incremental_speed_max_duration 
		);

		OnMovement();
	}

    void OnGravity()
    {
		speed_current = Mathf.Max( 
            speed_current - Time.deltaTime * movement_data.incremental_speed_min_duration,
            -movement_data.incremental_speed_min
        );

		OnMovement();
	}

	void OnMovement()
	{
		var position = transform.position;
		position   += Vector3.up * speed_current * Time.deltaTime;
		position.y  = Mathf.Max( falldown_position, position.y );

		if( Mathf.Approximately( position.y, falldown_position ) )
			DoIdle();
		else
		{
			transform.position = position;
			rotate_transform.Rotate( Vector3.up * speed_current * GameSettings.Instance.movement_rotation_cofactor, Space.Self );
		}
	}

	void DoRotate()
	{
		rotate_transform.Rotate( Vector3.up * speed_current * GameSettings.Instance.movement_rotation_cofactor, Space.Self );
	}

	void OnPathComplete()
	{
		updateMethod  = OnMovement;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
