/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Movement : MonoBehaviour
{
#region Fields
    //todo get this info from Incremental
    [ ShowInInspector ] float maxSpeed;

	// Private Fields
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
    public void Idle()
    {
		updateMethod  = ExtensionMethods.EmptyMethod;
		speed_current = 0;
	}

    public void StartAcceleration( float fallDownPosition )
    {
		falldown_position = fallDownPosition;
		speed_current     = 0;
		updateMethod      = OnAcceleration;
	}

    public void StopAcceleration()
    {
		speed_current = 0;
		updateMethod  = OnGravity;
	}
#endregion

#region API
#endregion

#region Implementation
    void OnAcceleration()
    {
		speed_current = Mathf.Min( 
			maxSpeed, 
			speed_current + Time.deltaTime * maxSpeed / GameSettings.Instance.acceleration_duration 
		);

		ChangePosition();
	}

    void OnGravity()
    {
		speed_current = Mathf.Max( 
            speed_current - Time.deltaTime * GameSettings.Instance.falldown_gravity,
            -GameSettings.Instance.fallDown_speed_max
        );

		ChangePosition();
	}

	void ChangePosition()
	{
		var position = transform.position;
		position   += Vector3.up * speed_current * Time.deltaTime;
		position.y  = Mathf.Max( falldown_position, position.y );

		transform.position = position;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
