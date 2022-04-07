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
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
