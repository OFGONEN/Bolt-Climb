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
  [ BoxGroup( "Shared Variables" ) ]
    [ SerializeField ] Currency property_currency;
    [ SerializeField ] Durability property_durability;
    [ SerializeField ] Velocity property_velocity;
	[ SerializeField ] SharedFloat shared_velocity_pathSpeed;
	[ SerializeField ] SharedFloat shared_velocity_gravity;

  [ BoxGroup( "Setup" ) ]
    [ SerializeField ] Color skill_currency_on_newBolt_color;
    [ SerializeField ] Vector2 skill_currency_on_newBolt_size;

  [ FoldoutGroup( "Currency Skills" ) ]
    [ SerializeField ] SkillData skill_currency_on_newBolt;
    [ SerializeField ] SkillData skill_currency_on_maxSpeed;
    [ SerializeField ] SkillData skill_currency_on_air;

  [ FoldoutGroup( "Durability Skills" ) ]
    [ SerializeField ] SkillData skill_durability_on_newBolt;
    [ SerializeField ] SkillData skill_durability_on_maxSpeed;
    [ SerializeField ] SkillData skill_durability_on_path;

  [ FoldoutGroup( "Speed Skills" ) ]
    [ SerializeField ] SkillData skill_velocity_on_newBolt;
    [ SerializeField ] SkillData skill_velocity_gravity;
    [ SerializeField ] SkillData skill_velocity_path;

  [ FoldoutGroup( "Last Chance Skills" ) ]
    [ SerializeField ] SkillData skill_lastChance_doubleJump;
    [ SerializeField ] SkillData skill_lastChance_Shatter;
#endregion

#region Properties
#endregion

#region Unity API
    public void OnAwake()
    {

    }
#endregion

#region API
	public void OnLevel_Revealed()
	{
		FFLogger.Log( "Revealed" );

		if( skill_velocity_gravity.IsUnlocked )
			shared_velocity_gravity.sharedValue = skill_velocity_gravity.Value;
		
		if( skill_velocity_path.IsUnlocked )
			shared_velocity_pathSpeed.sharedValue = skill_velocity_path.Value;
	}

    public void OnNutAttachedBolt()
    {
        if( skill_currency_on_newBolt.IsUnlocked )
			property_currency.OnIncrease( skill_currency_on_newBolt.Value, skill_currency_on_newBolt_color, skill_currency_on_newBolt_size );

        if( skill_durability_on_newBolt.IsUnlocked )
			property_durability.OnIncrease( skill_durability_on_newBolt.Value );

        if( skill_velocity_on_newBolt.IsUnlocked )
			property_velocity.OnAcceleration( skill_velocity_on_newBolt.Value );
	}

    public void OnMaxSpeed()
    {
        if( skill_currency_on_maxSpeed.IsUnlocked )
			property_currency.OnIncrease( skill_currency_on_maxSpeed.Value, skill_currency_on_newBolt_color, skill_currency_on_newBolt_size );

        if( skill_durability_on_maxSpeed.IsUnlocked )
			property_durability.OnIncrease( skill_durability_on_maxSpeed.Value );
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
