/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
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
	[ SerializeField ] SharedBoolNotifier notif_nut_IsOnBolt;
    [ SerializeField ] GameEvent event_nut_shatter;

  [ BoxGroup( "Setup" ) ]
    [ SerializeField ] Color skill_currency_on_newBolt_color;
    [ SerializeField ] Vector2 skill_currency_on_newBolt_size;
    [ SerializeField ] float skill_lastChange_shatter_duration;

    [ FoldoutGroup( "Currency Skills" ), SerializeField ] SkillData skill_currency_on_newBolt;
    [ FoldoutGroup( "Currency Skills" ), SerializeField ] SkillData skill_currency_on_maxSpeed;
    [ FoldoutGroup( "Currency Skills" ), SerializeField ] SkillData skill_currency_on_air;

    [ FoldoutGroup( "Durability Skills" ), SerializeField ] SkillData skill_durability_on_newBolt;
    [ FoldoutGroup( "Durability Skills" ), SerializeField ] SkillData skill_durability_on_maxSpeed;
    [ FoldoutGroup( "Durability Skills" ), SerializeField ] SkillData skill_durability_on_path;

    [ FoldoutGroup( "Speed Skills" ), SerializeField ] SkillData skill_velocity_on_newBolt;
    [ FoldoutGroup( "Speed Skills" ), SerializeField ] SkillData skill_velocity_gravity;
    [ FoldoutGroup( "Speed Skills" ), SerializeField ] SkillData skill_velocity_path;

    [ FoldoutGroup( "Last Chance Skills" ), SerializeField ] SkillData skill_lastChance_doubleJump;
    [ FoldoutGroup( "Last Chance Skills" ), SerializeField ] SkillData skill_lastChance_Shatter;
// Delegates
	UnityMessage onUpdate_NutPath;
	UnityMessage onUpdate_NutAir;
	UnityMessage onFinger_Down;

	[ ShowInInspector, ReadOnly ] bool canJump;
#endregion

#region Properties
#endregion

#region Unity API
    public void OnAwake()
    {
		onUpdate_NutPath = ExtensionMethods.EmptyMethod;
		onUpdate_NutAir  = ExtensionMethods.EmptyMethod;
		onFinger_Down    = ExtensionMethods.EmptyMethod;
	}

#endregion

#region API
	public void OnLevel_Finished()
	{
		onUpdate_NutPath = ExtensionMethods.EmptyMethod;
		onUpdate_NutAir  = ExtensionMethods.EmptyMethod;
		onFinger_Down    = ExtensionMethods.EmptyMethod;
	}

	public void OnLevel_Revealed()
	{
		if( skill_velocity_gravity.IsUnlocked )
			shared_velocity_gravity.sharedValue = skill_velocity_gravity.Value;
		
		if( skill_velocity_path.IsUnlocked )
			shared_velocity_pathSpeed.sharedValue = skill_velocity_path.Value;

		if( skill_durability_on_path.IsUnlocked )
			onUpdate_NutPath = Nut_PathUpdate;

		if( skill_currency_on_air.IsUnlocked )
			onUpdate_NutAir = Nut_AirUpdate;
		
		if( skill_lastChance_doubleJump.IsUnlocked )
			onFinger_Down = Finger_Down;

		canJump = true;
	}

    public void OnNutAttachedBolt()
    {
        if( skill_currency_on_newBolt.IsUnlocked )
			property_currency.OnIncrease( skill_currency_on_newBolt.Value, skill_currency_on_newBolt_color, skill_currency_on_newBolt_size );

        if( skill_durability_on_newBolt.IsUnlocked )
			property_durability.OnIncreaseCapacity( skill_durability_on_newBolt.Value );

        if( skill_velocity_on_newBolt.IsUnlocked )
			property_velocity.OnAcceleration( skill_velocity_on_newBolt.Value );
	}

    public void OnMaxSpeed()
    {
        if( skill_currency_on_maxSpeed.IsUnlocked )
			property_currency.OnIncrease( skill_currency_on_maxSpeed.Value, skill_currency_on_newBolt_color, skill_currency_on_newBolt_size );

        if( skill_durability_on_maxSpeed.IsUnlocked )
			property_durability.OnIncreaseCapacity( skill_durability_on_maxSpeed.Value );
	}

	public void OnNut_PathUpdate()
	{
		onUpdate_NutPath();
	}

	public void OnNut_AirUpdate()
	{
		onUpdate_NutAir();
	}

	public void OnFinger_Down()
	{
		onFinger_Down();
	}

	public void OnIsNutOnBoltChange( bool value )
	{
		canJump = canJump || value;
	}

	public void OnNut_DurabilityDeplate()
	{
		if( skill_lastChance_Shatter.IsUnlocked )
		{
			FFLogger.Log( "Delay Shatter" );
			property_velocity.OnAcceleration( skill_lastChance_Shatter.Value );
			DOVirtual.DelayedCall( skill_lastChange_shatter_duration, event_nut_shatter.Raise );
		}
		else
			event_nut_shatter.Raise();
	}
#endregion

#region Implementation
	void Nut_PathUpdate()
	{
		property_durability.OnIncrease( skill_durability_on_path.Value );
	}

	void Nut_AirUpdate()
	{
		property_currency.OnIncreaseCooldown( skill_currency_on_air.Value, skill_currency_on_newBolt_color, skill_currency_on_newBolt_size );
	}

	void Finger_Down()
	{
		if( canJump && !notif_nut_IsOnBolt.SharedValue )
		{
			property_velocity.OnAcceleration( skill_lastChance_doubleJump.Value );
			canJump = false;
		}
	}
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