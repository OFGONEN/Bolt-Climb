/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "skill_system", menuName = "FF/Skill System" ) ]
public class SkillSystem : ScriptableObject
{
#region Fields
  [ Title( "Currency" ) ]
    [ SerializeField ] SkillData skill_currency_on_newBolt;
    [ SerializeField ] SkillData skill_currency_on_maxSpeed;
    [ SerializeField ] SkillData skill_currency_on_air;

  [ Title( "Durability" ) ]
    [ SerializeField ] SkillData skill_durability_on_newBolt;
    [ SerializeField ] SkillData skill_durability_on_maxSpeed;
    [ SerializeField ] SkillData skill_durability_on_path;

  [ Title( "Speed" ) ]
    [ SerializeField ] SkillData skill_velocity_on_newBolt;
    [ SerializeField ] SkillData skill_velocity_gravity;
    [ SerializeField ] SkillData skill_velocity_path;

  [ Title( "Last Chance" ) ]
    [ SerializeField ] SkillData skill_lastChance_doubleJump;
    [ SerializeField ] SkillData skill_lastChance_Shatter;


  [ Title( "Shared Variables" ) ]
    [ SerializeField ] Currency property_currency;
    [ SerializeField ] Durability property_durability;
    [ SerializeField ] Velocity property_velocity;


  [ FoldoutGroup( "Setup" ) ]
    [ SerializeField ] Color skill_currency_on_newBolt_color;
    [ SerializeField ] Vector2 skill_currency_on_newBolt_size;
#endregion

#region Properties
#endregion

#region Unity API
    public void OnAwake()
    {

    }
#endregion

#region API
    public void OnNutAttachedBolt()
    {
        if( skill_currency_on_newBolt.IsUnlocked )
			property_currency.OnIncrease( skill_currency_on_newBolt.Value, skill_currency_on_newBolt_color, skill_currency_on_newBolt_size );

        if( skill_durability_on_newBolt.IsUnlocked )
			property_durability.OnIncrease( skill_durability_on_newBolt.Value );

        if( skill_velocity_on_newBolt.IsUnlocked )
			property_velocity.OnAcceleration( skill_velocity_on_newBolt.Value );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnValidate()
	{
		skill_currency_on_newBolt_color = skill_currency_on_newBolt_color.SetAlpha( 1 );
	}
#endif
#endregion
}
