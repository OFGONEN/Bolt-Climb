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
    [ FoldoutGroup( "Setup" ) ] public float bolt_model_height = 0.5f; 


	[ FoldoutGroup( "Environment Setup" ) ] public GameObject prefab_environment_ground; 
	[ FoldoutGroup( "Environment Setup" ) ] public GameObject prefab_environment_background; 
    
    const char prefab_bolt_char = 'b';
    const char prefab_bolt_shaped_char = 'c';
    const char space_char = 's';

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
	[ Button() ]
	public void SetIncremental_Velocity( int index )
	{
		PlayerPrefs.SetInt( ExtensionMethods.velocity_index, Mathf.Max( index, 0 ) );
	}

	[ Button() ]
	public void SetIncremental_Durability( int index )
	{
		PlayerPrefs.SetInt( ExtensionMethods.durability_index, Mathf.Max( index, 0 ) );
	}

	[ Button() ]
	public void SetIncremental_Currency( int index )
	{
		PlayerPrefs.SetInt( ExtensionMethods.currency_index, Mathf.Max( index, 0 ) );
	}
	
	[ Button() ]
	public void SetCurrency( float value )
	{
		PlayerPrefs.SetFloat( ExtensionMethods.currency, value );
	}
#endregion

#region Unity API
#endregion

#region API
	[ Button() ]
	public void CreateEnvironment( int backgroundCount )
	{
		EditorSceneManager.MarkAllScenesDirty();

		var parent = GameObject.Find( "environment_parent" ).transform;
		parent.DestoryAllChildren();

		Camera.main.transform.position = Camera.main.transform.position.SetY( 1.6f );

		// Spawn Ground
		var ground = PrefabUtility.InstantiatePrefab( prefab_environment_ground ) as GameObject;
		ground.transform.position = new Vector3( 0, -4.5f, -5f );
		ground.transform.SetParent( parent );
		ground.isStatic = true;

		for( var i = 0; i < backgroundCount; i++ )
		{
			var background = PrefabUtility.InstantiatePrefab( prefab_environment_background ) as GameObject;
			background.transform.position = new Vector3( 0, -3f, 2.5f ) + Vector3.up * 50 * i;
			background.transform.SetParent( parent );
			background.isStatic = true;
		}


		EditorSceneManager.SaveOpenScenes();
	}


    [ Button() ]
    public void CreateLevel()
    {
		EditorSceneManager.MarkAllScenesDirty();

		spawnTransform = GameObject.FindWithTag( "Respawn" ).transform;
		spawnTransform.DestoryAllChildren();

		// int errorIdex;

		// if( !IsCodeValid( out errorIdex ) )
        // {
        //     FFLogger.LogError( "CODE IS NOT VALID: " + errorIdex );
		// 	return;
		// }

		create_index = 0;
		create_length = level_start_bolt_length;
		create_position = 0;
		create_path_index = 0;

		// Place Start Bolt first
		var bolt_start = PrefabUtility.InstantiatePrefab( prefab_bolt_start ) as GameObject;
		bolt_start.transform.position = Vector3.up * create_position;
		bolt_start.transform.SetParent( spawnTransform );

		var bolt = bolt_start.GetComponent< Bolt >();
		bolt.PlaceBolts( Mathf.FloorToInt( create_length ), bolt_model_height, prefab_bolt_model );

		create_position += create_length * bolt_model_height + level_start_bolt_space;

		boltSpawn           = bolt;
		isSpawn_bolt        = true;
		isSpawn_consecutive = Mathf.Approximately( level_start_bolt_space, 0 );

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
		FFLogger.Log( $"Char {create_index}: " + level_code[ create_index ] );
        if( level_code[ create_index ] == 's' ) // Place Space
        {
			create_index++;
			FindLength();
			create_position += create_length;

			isSpawn_consecutive = Mathf.Approximately( 0, create_length );
		}
        else if( level_code[ create_index ] == 'b' ) // Place Bolt
        {
			create_index++;
			FindLength();
			PlaceBolt();
		}
        else if( level_code[ create_index ] == 'c' ) // Place S Shaped Bolt
        {
			PlaceShapedBolt();
			create_index++;

			isSpawn_bolt = false;
			isSpawn_consecutive = false;
		}
        else if( level_code[ create_index ] == 'e' ) // Place End Level Bolt
		{
			FFLogger.Log( "Place End Level" );
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

    void PlaceBolt()
    {
		var bolt_start = PrefabUtility.InstantiatePrefab( prefab_bolt ) as GameObject;
		bolt_start.transform.position = Vector3.up * create_position;
		bolt_start.transform.SetParent( spawnTransform );

		var bolt = bolt_start.GetComponent< Bolt >();
		bolt.PlaceBolts( Mathf.FloorToInt( create_length ), bolt_model_height, prefab_bolt_model );

		create_position += create_length * bolt_model_height;

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
	private void OnValidate()
	{
    	level_start_bolt_length = Mathf.Max( 2, level_start_bolt_length );
	}
#endif
#endregion
}