/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "level_creator_environment", menuName = "FFEditor/Level Creator Environment" ) ]
public class LevelCreatorEnvironment : ScriptableObject
{
#region Fields
  [ Title( "Create" ) ]
    [ SerializeField ] public EnvironmentData[] environmentData;

    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_ground;
    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_background;
    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_background_side;
    [ FoldoutGroup( "Setup" ) ] public float environment_offset;
    [ FoldoutGroup( "Setup" ) ] public float prefab_background_depth;
    [ FoldoutGroup( "Setup" ) ] public float prefab_background_height;
    [ FoldoutGroup( "Setup" ) ] public float prefab_background_side_depth;
    [ FoldoutGroup( "Setup" ) ] public float prefab_background_side_rightPosition;
    [ FoldoutGroup( "Setup" ) ] public float prefab_background_side_leftPosition;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API


    [ Button() ]
    public void CreateAllLevelEnvironment()
    {
        if( environmentData.Length > GameSettings.Instance.maxLevelCount )
        {
            FFLogger.LogError( "Environment Data count MUST BE EQUAL to Max Level Count" );
			return;
		}

        for( var i = 1; i <= environmentData.Length; i++ )
        {
			EditorSceneManager.OpenScene( EditorBuildSettings.scenes[ i ].path, OpenSceneMode.Single );
			CreateLevelEnvironment( i - 1 );
		}
    }

    public void CreateLevelEnvironment( int index )
    {
		EditorSceneManager.MarkAllScenesDirty();

		var environmentParent = GameObject.Find( "environment" ).transform;

		// Destory Objects
		environmentParent.DestoryAllChildren();

		var ground = PrefabUtility.InstantiatePrefab( prefab_ground ) as GameObject;
		ground.GetComponentInChildren< Renderer >().sharedMaterial = environmentData[ index ].level_material_ground;
		ground.transform.SetParent( environmentParent );
		ground.transform.localPosition = Vector3.zero;

		var backgroundCount = GameObject.Find( "finishLine" ).transform.position.y / prefab_background_height - 1;
		GameObject lastBackground = null;

		for( var i = 0; i < backgroundCount; i++ )
        {
			var background = PrefabUtility.InstantiatePrefab( prefab_background ) as GameObject;
            background.GetComponentInChildren< Renderer >().sharedMaterial = environmentData[ index ].level_material_background;

			background.transform.SetParent( environmentParent );
			background.transform.localPosition    = i * Vector3.up * prefab_background_height + Vector3.forward * prefab_background_depth;
			background.transform.localEulerAngles = Vector3.zero;
			lastBackground = background;

			var sideGround_left = PrefabUtility.InstantiatePrefab( prefab_background_side ) as GameObject;
            sideGround_left.GetComponentInChildren< Renderer >().sharedMaterial = environmentData[ index ].level_material_background;

			sideGround_left.transform.SetParent( environmentParent );
			sideGround_left.transform.localPosition    = i * Vector3.up * prefab_background_height + Vector3.forward * prefab_background_side_depth + Vector3.right * prefab_background_side_leftPosition;
			sideGround_left.transform.localEulerAngles = Vector3.zero;

			var sideGround_right = PrefabUtility.InstantiatePrefab( prefab_background_side ) as GameObject;
            sideGround_right.GetComponentInChildren< Renderer >().sharedMaterial = environmentData[ index ].level_material_background;

			sideGround_right.transform.SetParent( environmentParent );
			sideGround_right.transform.localPosition    = i * Vector3.up * prefab_background_height + Vector3.forward * prefab_background_side_depth + Vector3.right * prefab_background_side_rightPosition;
			sideGround_right.transform.localEulerAngles = Vector3.zero;
		}

		environmentParent.transform.position = Vector3.up * environment_offset;
		EditorSceneManager.SaveOpenScenes();
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}


[ Serializable ]
public struct EnvironmentData
{
	public Material level_material_ground;
	public Material level_material_background;
}