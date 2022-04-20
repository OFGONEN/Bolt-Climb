/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "incremental_currency", menuName = "FF/Data/Incremental/Currency" ) ]
public class IncrementalCurrency : ScriptableObject
{
	[ SerializeField ] IncrementalCurrencyData[] incremental_data;

	public int IncrementalCount => incremental_data.Length;

    public IncrementalCurrencyData ReturnIncremental( int index )
    {
		return incremental_data[ index ];
	}
}
