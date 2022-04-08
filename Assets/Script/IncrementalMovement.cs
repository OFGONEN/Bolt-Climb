/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "incremental_movement", menuName = "FF/Data/Incremental/Movement" ) ]
public class IncrementalMovement : ScriptableObject
{
	[ SerializeField ] IncrementalMovementData[] incremental_data;

    public IncrementalMovementData ReturnIncremental( int index )
    {
		return incremental_data[ index ];
	}
}
