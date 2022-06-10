/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "skin_library", menuName = "FF/Data/Skin Library" ) ]
public class SkinLibrary : ScriptableObject
{
    [ SerializeField ] Skin[] skin_datas;

    public Mesh GetMesh()
    {
		return skin_datas[ PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.nut_geometry_index, 0 ) ].SkinMesh;
	}

    public Material GetMaterial()
    {
		return GetSkinData().skin_material;
	}

	public Color GetCrackColor()
	{
		return GetSkinData().skin_crack_color;
	}

	SkinData GetSkinData()
	{
		var index_geometry = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.nut_geometry_index, 0 );
		var index_skin     = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.nut_skin_index, 0 );
		return skin_datas[ index_geometry ].GetData( index_skin );
	}
}
