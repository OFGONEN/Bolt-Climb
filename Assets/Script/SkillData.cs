/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "skill_", menuName = "FF/Data/Skill" ) ]
public class SkillData : ScriptableObject
{
#region Fields
  // Public
	public SkillValue[] skill_value_array;

  // Setup
	[ FoldoutGroup( "Setup" ), SerializeField ] Currency currency; 
	[ FoldoutGroup( "Setup" ), SerializeField ] Sprite skill_texture;
	[ FoldoutGroup( "Setup" ), SerializeField ] string skill_description;
	[ FoldoutGroup( "Setup" ), SerializeField ] string skill_key;
  // Private
	int skill_index;

  // Properties
	public bool IsUnlocked
	{
		get
		{
			skill_index = PlayerPrefsUtility.Instance.GetInt( skill_key, -1 );
			return skill_index != -1;
		}
	}

	public float Index => skill_index;
	public float Value => skill_value_array[ skill_index ].value;
	public float Cost => skill_value_array[ skill_index ].cost;
#endregion

#region API 
	[ Button() ]
	public void Unlock()
	{
		skill_index = 0;
		PlayerPrefsUtility.Instance.SetInt( skill_key, skill_index );
		currency.SharedValue -= Cost;
	}
#endregion
}