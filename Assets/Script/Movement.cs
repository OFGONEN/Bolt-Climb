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

  [ Title( "Setup" ) ]
	public Transform rotate_transform;
	// Private Fields
	[ ShowInInspector, ReadOnly ] float speed_max;
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
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region Unity API
    public void StartAcceleration( float fallDownPosition )
    {
		speed_max = 5f;

		falldown_position = fallDownPosition;
		speed_current      = 0;
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
			speed_max, 
			speed_current + Time.deltaTime * speed_max / GameSettings.Instance.acceleration_duration 
		);

		OnMovement();
	}

    void OnGravity()
    {
		speed_current = Mathf.Max( 
            speed_current - Time.deltaTime * GameSettings.Instance.falldown_acceleration,
            -GameSettings.Instance.fallDown_speed_max
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
			rotate_transform.Rotate( Vector3.up * speed_current * GameSettings.Instance.rotation_cofactor, Space.Self );
		}
	}

	void DoRotate()
	{
		rotate_transform.Rotate( Vector3.up * speed_current * GameSettings.Instance.rotation_cofactor, Space.Self );
	}

	void OnPathComplete()
	{
		speed_current = GameSettings.Instance.launch_speed;
		updateMethod  = OnMovement;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
