/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Nut : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" )]
	[ SerializeField ] ShatterRandomPool pool_randomShatter;
	[ SerializeField ] GameEvent event_level_failed;
	[ SerializeField ] GameEvent event_level_completed;

  [ Title( "Components" )]
	[ SerializeField ] Movement component_movement;
	[ SerializeField ] AnimationHandle component_animation;
	[ SerializeField ] Velocity property_velocity;
	[ SerializeField ] Durability property_durability;
	[ SerializeField ] Currency property_currency;
// Private
	float point_fallDown = 0;

// Delegates
	UnityMessage onUpdateMethod;
	UnityMessage onFingerDown;
	UnityMessage onFingerUp;
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		onUpdateMethod = ExtensionMethods.EmptyMethod;
		onFingerDown   = ExtensionMethods.EmptyMethod;
		onFingerUp     = ExtensionMethods.EmptyMethod;
	}

	private void Update()
	{
		onUpdateMethod();
	}
#endregion

#region API
	public void Input_OnFingerDown()
	{
		onFingerDown();
	}

	public void Input_OnFingerUp()
	{
		onFingerUp();
	}

	public void OnLevelStarted()
	{
		// Set properties up
		property_currency.SetCurrencyData();
		property_velocity.SetVelocityData();
		property_durability.SetDurabilityData();

		onFingerDown = OnFingerDown_StraightBolt;
	}

	public void OnIsNutOnBoltChange( bool value )
	{
		if( value )
			onFingerDown = OnFingerDown_StraightBolt;
		else
			onFingerDown = ExtensionMethods.EmptyMethod;
	}

	public void OnFallDownPointChange( float value )
	{
		point_fallDown = value;
	}

	public void OnShapedBolt( IntGameEvent gameEvent )
	{
		EmptyDelegates();
		component_movement.DoPath( GameSettings.Instance.movement_path_shaped, OnPathComplete );
	}

	public void OnLevelEndBolt( IntGameEvent gameEvent )
	{
		EmptyDelegates();
		component_movement.DoPath( GameSettings.Instance.movement_path_end, OnLevelEndPathComplete );
	}
#endregion

#region Implementation
	void OnPathComplete()
	{
		var position   = transform.position;
		    position.x = 0;
		    position.z = 0;

		transform.position = position; // todo remove this after path points are corrected

		onUpdateMethod = OnUpdate_Deceleration;
	}

	void OnLevelEndPathComplete()
	{
		event_level_completed.Raise();
	}

	void OnUpdate_Idle()
	{
		property_durability.OnIncrease();
		component_animation.PlayAnimation( property_durability.DurabilityRatio );
	}

	void OnUpdate_Acceleration()
	{
		if( Mathf.Approximately( 0, property_durability.CurrentDurability ) )
		{
			EmptyDelegates();
			gameObject.SetActive( false );

			var shatter                    = pool_randomShatter.GetEntity();
			    shatter.transform.position = transform.position;

			shatter.DoShatter();

			DOVirtual.DelayedCall( GameSettings.Instance.nut_shatter_waitDuration, event_level_failed.Raise );
		}
		else
		{
			property_velocity.OnAcceleration();
			component_movement.OnMovement();
			property_durability.OnDecrease();
			component_animation.PlayAnimation( property_durability.DurabilityRatio );
			property_currency.OnIncrease();
		}
	}

	void OnUpdate_Deceleration()
	{
		property_velocity.OnDeceleration();
		var isIdle = component_movement.OnMovement( point_fallDown );

		property_durability.OnIncrease();
		component_animation.PlayAnimation( property_durability.DurabilityRatio );

		if( isIdle )
			onUpdateMethod = OnUpdate_Idle;
	}

	void OnFingerDown_StraightBolt()
	{
		onUpdateMethod = OnUpdate_Acceleration;
		onFingerUp     = OnFingerUp;
	}

	void OnFingerUp()
	{
		onUpdateMethod = OnUpdate_Deceleration;
		onFingerUp     = ExtensionMethods.EmptyMethod;
	}

	void EmptyDelegates()
	{
		onUpdateMethod = ExtensionMethods.EmptyMethod;
		onFingerUp     = ExtensionMethods.EmptyMethod;
		onFingerDown   = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
//! todo remove this variable before build
	[ SerializeField ] SharedBoolNotifier isNutOnBolt;

	private void OnGUI() 
	{
		var style = new GUIStyle();
		style.fontSize = 25;

		GUI.Label( new Rect( 25, 50 , 250, 250 ), "Is Nut On Bolt: " + isNutOnBolt.SharedValue  , style);
		GUI.Label( new Rect( 25, 75 , 250, 250 ), "Nut Durability: " + property_durability.CurrentDurability , style);
		GUI.Label( new Rect( 25, 100, 250, 250 ), "Nut %Durability: " + property_durability.DurabilityRatio , style);
		GUI.Label( new Rect( 25, 125, 250, 250 ), "Nut Velocity: " + property_velocity.CurrentVelocity , style);
		GUI.Label( new Rect( 25, 150, 250, 250 ), "Curreny: " + property_currency.SharedValue , style);
	}
#endif
#endregion
}