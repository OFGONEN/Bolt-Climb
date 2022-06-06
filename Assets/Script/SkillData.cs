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
	public SkillValue[] skill_value_array;
	public float skill_value_default;
	public Sprite skill_texture;
	public string skill_description;
	public string skill_key;
	public UnityEvent skill_OnUnlock;
#endregion
}