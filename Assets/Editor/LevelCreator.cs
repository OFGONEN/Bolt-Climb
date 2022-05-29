/* Created by and for usage of FF Studios (2021). */

using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;
using FFEditor;
using FFStudio;

[ CreateAssetMenu( fileName = "level_creator", menuName = "FFEditor/LevelCreator" ) ]
public class LevelCreator : ScriptableObject
{
#region Fields
  [ Title( "Create" ) ]
    public string level_code;
  [ Title( "Create" ) ]
    public int level_start_bolt_length;
    public float level_start_bolt_space;

    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_bolt_model; 
	[ FoldoutGroup( "Setup" ) ] public GameObject prefab_bolt; 
    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_bolt_start; 
    [ FoldoutGroup( "Setup" ) ] public GameObject[] prefab_bolt_shaped; 
    [ FoldoutGroup( "Setup" ) ] public float[] bolt_shaped_model_height; // 28 
    [ FoldoutGroup( "Setup" ) ] public GameObject[] prefab_bolt_end; 
    [ FoldoutGroup( "Setup" ) ] public Vector3[] prefab_finishLine_offset; 
    [ FoldoutGroup( "Setup" ) ] public GameObject prefab_finishLine; 

    const char char_bolt        = 'b';
    const char char_bolt_shaped = 'c';
    const char char_bolt_end    = 'e';
    const char char_space       = 'g';

	Transform spawnTransform;
	Bolt boltSpawn;
	bool isSpawn_bolt;
	bool isSpawn_consecutive;

	int create_index = 0;
	float create_length = 0;
	float create_position = 0;
	int create_path_index = 0;

	StringBuilder stringBuilder = new StringBuilder( 128 );
#endregion

#region PlayerPrefs
#endregion

#region Unity API
#endregion

#region API

    [ Button() ]
    public void CreateLevel()
    {
		EditorSceneManager.MarkAllScenesDirty();

		spawnTransform = GameObject.FindWithTag( "Respawn" ).transform;
		spawnTransform.DestoryAllChildren();

		create_index      = 0;
		create_length     = 0;
		create_position   = 0;
		create_path_index = 0;

		if( level_start_bolt_length <= GameSettings.Instance.bolt_batch )
		{
			FFLogger.LogError( "Level Start Bolt Length CAN NOT BE SMALLER THAN Bolt Batch size" );
			return;
		}

		// Place Start Bolt Start
		var bolt_start = PrefabUtility.InstantiatePrefab( prefab_bolt_start ) as GameObject;
		bolt_start.transform.position = Vector3.up * create_position;
		bolt_start.transform.SetParent( spawnTransform );

		var bolt = bolt_start.GetComponent< Bolt >();
		bolt.PlaceBolts( GameSettings.Instance.bolt_batch, GameSettings.Instance.bolt_height, prefab_bolt_model );

		boltSpawn           = bolt;
		isSpawn_bolt        = true;
		isSpawn_consecutive = true;

		create_position += GameSettings.Instance.bolt_batch * GameSettings.Instance.bolt_height;

		var length = level_start_bolt_length - GameSettings.Instance.bolt_batch;

		for( var i = 0; i < length / GameSettings.Instance.bolt_batch; i++ )
		{
			PlaceBolt( GameSettings.Instance.bolt_batch );
		}

		var mod = length % GameSettings.Instance.bolt_batch;

		if( mod > 0 )
			PlaceBolt( mod );

		create_position += level_start_bolt_space;
		isSpawn_consecutive = Mathf.Approximately( level_start_bolt_space, 0 );
		// Place Start Bolt End

		while( create_index < level_code.Length - 1 )
        {
			PlaceObject();
		}

		EditorSceneManager.SaveOpenScenes();
	}
#endregion

#region Implementation
    void PlaceObject()
    {
        if( level_code[ create_index ] == char_space ) // Place Space
        {
			create_index++;
			FindLength();
			create_position += create_length;

			isSpawn_consecutive = Mathf.Approximately( 0, create_length );
		}
        else if( level_code[ create_index ] == char_bolt ) // Place Bolt
        {
			create_index++;
			FindLength();

			var length = Mathf.FloorToInt( create_length );

			for( var i = 0; i < length / GameSettings.Instance.bolt_batch; i++ )
			{
				PlaceBolt( GameSettings.Instance.bolt_batch );
			}

			var mod = length % GameSettings.Instance.bolt_batch;

			if( mod > 0 )
				PlaceBolt( mod );
		}
        else if( level_code[ create_index ] == char_bolt_shaped ) // Place S Shaped Bolt
        {
			PlaceShapedBolt();
			create_index++;

			isSpawn_bolt = false;
			isSpawn_consecutive = false;
		}
        else if( level_code[ create_index ] == char_bolt_end ) // Place End Level Bolt
		{
			create_index = create_index + 1;
			var bolt_end_index = int.Parse( level_code[ create_index ].ToString() );

			// Place Level End Bolt
			var bolt_end = PrefabUtility.InstantiatePrefab( prefab_bolt_end[ bolt_end_index ] ) as GameObject;
			bolt_end.transform.position = Vector3.up * create_position;
			bolt_end.transform.SetParent( spawnTransform );

			var path = bolt_end.GetComponent<MovementPath>();
			path.path_index = create_path_index;
			path.MovePoints();

			var finishLine = PrefabUtility.InstantiatePrefab( prefab_finishLine ) as GameObject;
			finishLine.transform.position = bolt_end.transform.position + prefab_finishLine_offset[ bolt_end_index ];
			finishLine.transform.SetParent( spawnTransform );

			if( isSpawn_bolt )
				bolt_end.GetComponent< BoltDetach >().ConnectBolt( boltSpawn );

			isSpawn_bolt = false;
			isSpawn_consecutive = false;
		}
    }

    void FindLength()
    {
		stringBuilder.Clear();

		for( var i = create_index; i < level_code.Length; i++ )
        {
			stringBuilder.Append( level_code[ i ] );
			create_index = i + 1;

            if( i < level_code.Length - 1 && IsSpecial( i + 1 ) )
				break;
		}

		create_length = float.Parse( stringBuilder.ToString() );
	}

    void PlaceBolt( int count )
    {
		var bolt_start = PrefabUtility.InstantiatePrefab( prefab_bolt ) as GameObject;
		bolt_start.transform.position = Vector3.up * create_position;
		bolt_start.transform.SetParent( spawnTransform );

		var bolt = bolt_start.GetComponent< Bolt >();
		// bolt.PlaceBolts( Mathf.FloorToInt( create_length ), bolt_model_height, prefab_bolt_model );
		bolt.PlaceBolts( count, GameSettings.Instance.bolt_height, prefab_bolt_model );

		create_position += count * GameSettings.Instance.bolt_height;

		if( isSpawn_bolt )
		{
			bolt.ConnectBolt( boltSpawn, isSpawn_consecutive );
		}

		boltSpawn           = bolt;
		isSpawn_bolt        = true;
		isSpawn_consecutive = true;
	}

    void PlaceShapedBolt()
    {
		create_index = create_index + 1;
		var bolt_shaped_index = int.Parse( level_code[ create_index ].ToString() );
		var bolt_shaped = PrefabUtility.InstantiatePrefab( prefab_bolt_shaped[ bolt_shaped_index] ) as GameObject;
		bolt_shaped.transform.position = Vector3.up * create_position;
		bolt_shaped.transform.SetParent( spawnTransform );

		var path = bolt_shaped.GetComponent< MovementPath >();
        path.path_index = create_path_index;
        path.MovePoints();

		create_path_index++;
		create_position += bolt_shaped_model_height[ bolt_shaped_index ];

		if( isSpawn_bolt )
			bolt_shaped.GetComponent< BoltDetach >().ConnectBolt( boltSpawn );
	}

    bool IsCodeValid( out int errorIdex )
    {
		level_code = level_code.ToLower();

		     errorIdex        = -1;
		bool result           = true;

		for( var i = 0; i < level_code.Length; i++ )
        {
			errorIdex = i;
            if( IsSpecial( i ) )
            {
				i++;
				while( i < level_code.Length && !IsSpecial( i ) )
                {
                    if( IsNumber( i ) )
						i++;
                    else
                    {
                        FFLogger.LogError( "INVALID CODE: " + errorIdex );
						return false;
					}
				}

				i--;
			}
            else
            {
                FFLogger.LogError( "INVALID CODE: " + errorIdex );
				return false;
			}
		}

		return result;
	}

    bool IsNumber( int index )
    {
		var codeChar = level_code[ index ];
		return codeChar == '.' || ( codeChar <= 57 && codeChar >= 48 );
	}

    bool IsSpecial( int index )
    {
		var codeChar = level_code[ index ];
		return codeChar == 'b' || codeChar == 'c' || codeChar == 's' || codeChar == 'e';
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}