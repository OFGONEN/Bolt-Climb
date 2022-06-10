/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "skin_library", menuName = "FF/Data/Skin Library" ) ]
public class SkinLibrary : ScriptableObject
{
	[ SerializeField ] Mesh[] skin_meshes;
	[ SerializeField ] string[] skin_geometry_name;
	[ SerializeField ] SkinData[] skin_data_array;
    [ SerializeField ] Skin[] skin_store_datas;

    public Mesh GetMesh()
    {
		return skin_meshes[ PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.nut_geometry_index, 0 ) ];
	}

    public SkinData GetSkinData()
    {
		return skin_data_array[ PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.nut_skin_index, 0 ) ];
	}

	public string GetGeometryName()
	{
		return skin_geometry_name[ PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.nut_geometry_index, 0 ) ];
	}
}