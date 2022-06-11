/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System.IO;
using System.Collections;

namespace FFStudio
{
	[ CreateAssetMenu( fileName = "LevelData", menuName = "FF/Data/LevelData" ) ]
	public class LevelData : ScriptableObject
    {
	[ Title( "Setup" ) ]
		[ ValueDropdown( "SceneList" ), LabelText( "Scene Index" ) ] public int scene_index;
        [ LabelText( "Override As Active Scene" ) ] public bool scene_overrideAsActiveScene;

	[ Title( "Incremental" ) ]
		[ LabelText( "Velocity Incremental Cap" ) ] public int incremental_cap_velocity = 100;
		[ LabelText( "Durability Incremental Cap" ) ] public int incremental_cap_durability = 100;
		[ LabelText( "Currency Incremental Cap" ) ] public int incremental_cap_currency = 100;
        [ LabelText( "Set Incremental" ) ] public bool incremental_set;
        [ LabelText( "Set Index of Velocity Incremental" ), ShowIf( "incremental_set" ) ] public int incremental_set_index_velocity;
        [ LabelText( "Set Index of Durability Incremental" ), ShowIf( "incremental_set" ) ] public int incremental_set_index_durability;

	[ Title( "Level Related" ) ]
		[ LabelText( "Type of Skill will be showned at the end of the Level" ) ] public SkillType skillType;
		[ LabelText( "Finish Line index to spawn" ) ] public int finishLineIndex;
		[ LabelText( "Nut geometry to set" ), ShowIf( "incremental_set" ) ] public int nut_geometry_index = 0;

	[ Title( "Progression" ) ]
		[ LabelText( "Level Progress Start Icon index" ) ] public int levelProgress_levelIcon_start_index;
		[ LabelText( "Level Progress End Icon index" ) ] public int levelProgress_levelIcon_end_index;
		[ LabelText( "Level Progress Nut Icon index" ) ] public int levelProgress_nutIcon_index;
		[ LabelText( "Base Level Progression" ) ] public float levelProgress_base;
		[ LabelText( "Current Level Progression" ) ] public float levelProgress_current = 0.25f;

#if UNITY_EDITOR
		private static IEnumerable SceneList()
        {
			var list = new ValueDropdownList< int >();

			var scene_count = SceneManager.sceneCountInBuildSettings;

			for( var i = 0; i < scene_count; i++ )
				list.Add( Path.GetFileNameWithoutExtension( SceneUtility.GetScenePathByBuildIndex( i ) ) + $" ({i})", i );

			return list;
		}
#endif
    }
}
