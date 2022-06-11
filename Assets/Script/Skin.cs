/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "skin_", menuName = "FF/Data/Skin" ) ]
public class Skin : ScriptableObject
{
#region Fields
	public SkinData_Store[] skin_data_store;
#endregion
}