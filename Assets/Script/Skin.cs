/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "skin_", menuName = "FF/Data/Skin" ) ]
public class Skin : ScriptableObject
{
#region Fields
    [ SerializeField ] SkinData[] skin_data_array;

    public SkinData GetData( int index )
    {
		return skin_data_array[ Mathf.Clamp( index, 0, skin_data_array.Length - 1 ) ];
	}
#endregion
}