/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "incremental_durability", menuName = "FF/Data/Incremental/Durability" ) ]
public class IncrementalDurability : ScriptableObject
{
	[ SerializeField ] IncrementalDurabilityData[] incremental_data;

    public IncrementalDurabilityData ReturnIncremental( int index )
    {
		return incremental_data[ index ];
	}
}
