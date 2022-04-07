/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Movement : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
	public Transform rotate_transform;
    //todo get this info from Incremental
    [ ShowInInspector ] float maxSpeed = 5;

	// Private Fields
	[ ShowInInspector, ReadOnly ] float currentSpeed;
	float falldown_position;

	// Delegates
	UnityMessage updateMethod;

    // Properties
    public float CurrentSpeed => currentSpeed;
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
		falldown_position = fallDownPosition;
		currentSpeed      = 0;
		updateMethod      = OnAcceleration;
	}

    public void StopAcceleration()
    {
		updateMethod  = OnGravity;
	}

	public void DoIdle()
	{
		currentSpeed = 0;
		updateMethod = ExtensionMethods.EmptyMethod;
	}
#endregion

#region API
#endregion

#region Implementation
    void OnAcceleration()
    {
		currentSpeed = Mathf.Min( 
			maxSpeed, 
			currentSpeed + Time.deltaTime * maxSpeed / GameSettings.Instance.acceleration_duration 
		);

		OnMovement();
	}

    void OnGravity()
    {
		currentSpeed = Mathf.Max( 
            currentSpeed - Time.deltaTime * GameSettings.Instance.falldown_acceleration,
            -GameSettings.Instance.fallDown_speed_max
        );

		OnMovement();
	}

	void OnMovement()
	{
		var position = transform.position;
		position   += Vector3.up * currentSpeed * Time.deltaTime;
		position.y  = Mathf.Max( falldown_position, position.y );

		if( Mathf.Approximately( position.y, falldown_position ) )
			DoIdle();
		else
		{
			transform.position = position;
			rotate_transform.Rotate( Vector3.up * currentSpeed * GameSettings.Instance.rotation_cofactor, Space.Self );
		}
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
