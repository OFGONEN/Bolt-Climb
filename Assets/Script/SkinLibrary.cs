/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "library_skin", menuName = "FF/Data/Skin Library" ) ]
public class SkinLibrary : ScriptableObject
{
    [ SerializeField ] SkinData[] skinDatas;


    public Mesh GetMesh( int index )
    {
		return skinDatas[ index ].skin_mesh;
	}

    public Material GetMaterial( int index )
    {
		return skinDatas[ index ].skin_material;
	}
}
