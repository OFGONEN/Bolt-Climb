/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FFStudio;

[ CreateAssetMenu( fileName = "skill_", menuName = "FF/Data/Skill" ) ]
public class SkillData : ScriptableObject
{
#region Fields

  // Public
	public SkillValue[] skill_value_array;
	public float skill_value_default;
	public Sprite skill_texture;
	public string skill_description;
	public string skill_key;
	public UnityEvent skill_OnUnlock;

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
}