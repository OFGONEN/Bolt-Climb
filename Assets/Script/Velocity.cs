/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "velocity", menuName = "FF/Data/Game/Velocity" ) ]
public class Velocity : ScriptableObject
{
#region Fields
    // Private
	[ SerializeField ] IncrementalVelocity velocity_incremental;
	[ ShowInInspector, ReadOnly ] IncrementalVelocityData velocity_data;
	[ ShowInInspector, ReadOnly ] float velocity_current;

    // Properties
    public float CurrentSpeed => velocity_current;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void SetVelocityData()
    {
		velocity_data = velocity_incremental.ReturnIncremental( PlayerPrefs.GetInt( "velocity_index", 0 ) );
	}

    public void OnAcceleration()
    {
		velocity_current = Mathf.Max( 0, velocity_current );;

		velocity_current = Mathf.Min( 
			velocity_data.incremental_velocity_max, 
			velocity_current + Time.deltaTime * velocity_data.incremental_velocity_max / velocity_data.incremental_velocity_max_duration 
		);
	}

    public void OnDeceleration()
    {
		velocity_current = Mathf.Max( 
            velocity_current - Time.deltaTime * velocity_data.incremental_velocity_decrease,
            velocity_data.incremental_velocity_min
        );
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
