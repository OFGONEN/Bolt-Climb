/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "durability", menuName = "FF/Data/Game/Durability" ) ]
public class Durability : ScriptableObject
{
#region Fields
    [ SerializeField ] IncrementalDurability durability_incremental;
    [ ShowInInspector, ReadOnly ] IncrementalDurabilityData durability_data;
    [ ShowInInspector, ReadOnly ] float durability_current_capacity;
    [ ShowInInspector, ReadOnly ] float durability_current;
#endregion

#region Properties
    // Properties
    public float CurrentDurability => durability_current;
    public float DurabilityRatio => durability_current / durability_data.incremental_durability_capacity;
#endregion

#region Unity API
#endregion

#region API
    public void SetDurabilityData()
    {
		durability_data             = durability_incremental.ReturnIncremental( PlayerPrefs.GetInt( "durability_index", 0 ) );
		durability_current_capacity = durability_data.incremental_durability_capacity;
		durability_current          = durability_data.incremental_durability_capacity;
	}

    public void OnIncrease()
    {
		durability_current = Mathf.Min( 
            durability_current + Time.deltaTime * durability_data.incremental_durability_speed_increase, 
            durability_current_capacity );
	}

    public void OnDecrease()
    {
		durability_current = Mathf.Max( 
            durability_current - Time.deltaTime * durability_data.incremental_durability_speed_decrease, 
            0 );

		durability_current_capacity = Mathf.Max(
			durability_current_capacity - Time.deltaTime * durability_data.incremental_durability_speed_capacity_decrease,
			0
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
