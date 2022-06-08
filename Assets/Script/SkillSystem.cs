/* Created by and for usage of FF Studios (2021). */

using System.Text;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "skill_system", menuName = "FF/Skill System" ) ]
public class SkillSystem : ScriptableObject
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] Color skill_currency_text_color;
    [ BoxGroup( "Setup" ), SerializeField ] Vector2 skill_currency_text_size;
    [ BoxGroup( "Setup" ), SerializeField ] Color skill_durability_text_color;
    [ BoxGroup( "Setup" ), SerializeField ] Vector2 skill_durability_text_size;
    [ BoxGroup( "Setup" ), SerializeField ] Color skill_speed_text_color;
    [ BoxGroup( "Setup" ), SerializeField ] Vector2 skill_speed_text_size;

    [ BoxGroup( "Settings" ), LabelText( "Shatter Dash Duration" ), SerializeField ] float skill_lastChange_shatter_duration;
    [ BoxGroup( "Settings" ), LabelText( "Cooldown for Gaining Durability on Path" ), SerializeField ] float skill_durability_on_path_cooldown;
    [ BoxGroup( "Settings" ), LabelText( "Cooldown for Gaining Currency on MaxSpeed" ), SerializeField ] float skill_currency_on_maxSpeed_cooldown;
    [ BoxGroup( "Settings" ), LabelText( "Cooldown for Gaining Durability on MaxSpeed" ), SerializeField ] float skill_durability_on_maxSpeed_cooldown;

    [ FoldoutGroup( "Shared Variables"), SerializeField ] Currency property_currency;
    [ FoldoutGroup( "Shared Variables"), SerializeField ] Durability property_durability;
    [ FoldoutGroup( "Shared Variables"), SerializeField ] Velocity property_velocity;
	[ FoldoutGroup( "Shared Variables"), SerializeField ] SharedFloat shared_velocity_pathSpeed;
	[ FoldoutGroup( "Shared Variables"), SerializeField ] SharedFloat shared_velocity_gravity;
	[ FoldoutGroup( "Shared Variables"), SerializeField ] SharedBoolNotifier notif_nut_IsOnBolt;
    [ FoldoutGroup( "Shared Variables"), SerializeField ] GameEvent event_nut_shatter;
    [ FoldoutGroup( "Shared Variables"), SerializeField ] GameEvent event_nut_jumped;
    [ FoldoutGroup( "Shared Variables"), SerializeField ] UICurrencyPool pool_currency_ui;

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

	StringBuilder stringBuilder = new StringBuilder( 16 );
	bool canJump;
	float pathDurabilityCooldown     = 0;
	float currencyMaxSpeedCooldown   = 0;
	float durabilityMaxSpeedCooldown = 0;

	RecycledTween recycledTween = new RecycledTween();
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
		else 
			shared_velocity_gravity.sharedValue = 1;

		
		if( skill_velocity_path.IsUnlocked )
			shared_velocity_pathSpeed.sharedValue = skill_velocity_path.Value;
		else
			shared_velocity_pathSpeed.sharedValue = 1;

		if( skill_durability_on_path.IsUnlocked )
			onUpdate_NutPath = Nut_PathUpdate_Start;

		if( skill_currency_on_air.IsUnlocked )
			onUpdate_NutAir = Nut_AirUpdate;
		
		if( skill_lastChance_doubleJump.IsUnlocked )
			onFinger_Down = Finger_Down;

		canJump                    = true;
		pathDurabilityCooldown     = 0;
		currencyMaxSpeedCooldown   = 0;
		durabilityMaxSpeedCooldown = 0;
	}

    public void OnNutAttachedBolt()
    {
        if( skill_currency_on_newBolt.IsUnlocked )
			property_currency.OnIncrease( skill_currency_on_newBolt.Value, skill_currency_text_color, skill_currency_text_size );

        if( skill_durability_on_newBolt.IsUnlocked )
		{
			var value = skill_durability_on_newBolt.Value;
			property_durability.OnIncreaseCapacity( value );
			pool_currency_ui.GetEntity().Spawn( $"Durability +{value}", skill_durability_text_color, skill_durability_text_size, -0.75f ); 
		}

        if( skill_velocity_on_newBolt.IsUnlocked )
		{
			var value = skill_velocity_on_newBolt.Value;
			property_velocity.OnAcceleration( value );
			pool_currency_ui.GetEntity().Spawn( $"Speed +{value}", skill_speed_text_color, skill_speed_text_size, 1.5f ); 
		}
	}

    public void OnMaxSpeed()
    {
        if( skill_currency_on_maxSpeed.IsUnlocked && Time.time > currencyMaxSpeedCooldown )
		{
			property_currency.OnIncrease( skill_currency_on_maxSpeed.Value, skill_currency_text_color, skill_currency_text_size );
			currencyMaxSpeedCooldown = Time.time + skill_currency_on_maxSpeed_cooldown;
		}

        if( skill_durability_on_maxSpeed.IsUnlocked && Time.time > durabilityMaxSpeedCooldown )
		{
			var value = skill_durability_on_maxSpeed.Value;
			property_durability.OnIncreaseCapacity( value );
			pool_currency_ui.GetEntity().Spawn( $"Durability +{value}", skill_durability_text_color, skill_durability_text_size, -0.75f );

			durabilityMaxSpeedCooldown = Time.time + skill_durability_on_maxSpeed_cooldown;
		}
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
			pool_currency_ui.GetEntity().Spawn( "Last Chance", Color.white, skill_durability_text_size ); 

			property_velocity.ZeroOutVelocity();
			property_velocity.OnAcceleration( skill_lastChance_Shatter.Value );

			recycledTween.Recycle( DOVirtual.DelayedCall( skill_lastChange_shatter_duration, event_nut_shatter.Raise ) );
		}
		else
			event_nut_shatter.Raise();
	}

	public void OnNutTriggersEndBolt()
	{
		recycledTween.Kill();
	}
#endregion

#region Implementation
	void Nut_PathUpdate_Start()
	{
		var durabilityValue = skill_durability_on_path.Value;
		pool_currency_ui.GetEntity().Spawn( $"Durability +{durabilityValue}", skill_durability_text_color, skill_durability_text_size, -0.75f ); 
		property_durability.OnIncrease( durabilityValue );

		if( skill_velocity_path.IsUnlocked )
		{
			var speedValue = skill_velocity_path.Value;
			pool_currency_ui.GetEntity().Spawn( $"Speed +{speedValue}", skill_durability_text_color, skill_durability_text_size, -0.75f ); 
		}

		onUpdate_NutPath = Nut_PathUpdate;
	}

	void Nut_PathUpdate()
	{
		if( Time.time > pathDurabilityCooldown )
		{
			var durabilityValue = skill_durability_on_path.Value;

			pool_currency_ui.GetEntity().Spawn( $"Durability +{durabilityValue}", skill_durability_text_color, skill_durability_text_size, -0.75f ); 
			property_durability.OnIncrease( durabilityValue  );

			pathDurabilityCooldown = Time.time + skill_durability_on_path_cooldown;
		}
	}

	void Nut_AirUpdate()
	{
		property_currency.OnIncreaseCooldown( skill_currency_on_air.Value, skill_currency_text_color, skill_currency_text_size );
	}

	void Finger_Down()
	{
		if( canJump && !notif_nut_IsOnBolt.SharedValue )
		{
			pool_currency_ui.GetEntity().Spawn( "Double Jump", Color.white, skill_durability_text_size ); 
			property_velocity.OnAcceleration( skill_lastChance_doubleJump.Value );
			canJump = false;
			event_nut_jumped.Raise();
		}
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnValidate()
	{
		skill_currency_text_color   = skill_currency_text_color.SetAlpha( 1 );
		skill_durability_text_color = skill_durability_text_color.SetAlpha( 1 );
		skill_speed_text_color      = skill_speed_text_color.SetAlpha( 1 );
	}
#endif
#endregion
}