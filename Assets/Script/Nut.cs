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
  [ Title( "Components" ) ]
    [ SerializeField ] Movement component_movement;
	[ SerializeField ] AnimationHandle component_animation;
	[ SerializeField ] Velocity property_velocity;
	[ SerializeField ] Durability property_durability;
	[ SerializeField ] Currency property_currency;
// Private
	float point_fallDown = 0;

// Delegates
    UnityMessage onUpdate;
	UnityMessage onFingerDown;
	UnityMessage onFingerUp;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		onUpdate     = ExtensionMethods.EmptyMethod;
		onFingerDown = ExtensionMethods.EmptyMethod;
		onFingerUp   = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		onUpdate();
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
		//todo: Set Properties( Velocity, Durability, Currency ) Up

		onFingerDown = OnInput_StraightBolt;
	}
#endregion

#region Implementation
//! Velocity
//! Movement
//! Durability
//! Animation
//! Currency
//! Nut Shatter - Level Fail
	void OnUpdate_Idle()
	{
		property_durability.OnIncrease();
		component_animation.PlayAnimation( property_durability.DurabilityRatio );
	}

	void OnUpdate_Acceleration()
	{
		property_velocity.OnAcceleration();
		component_movement.OnMovement( point_fallDown );
	}

	void OnUpdate_Deceleration()
	{
	}

	void OnInput_StraightBolt()
	{
	}

	void OnInput_ShapedBolt()
	{
	}

	public void OnIsNutOnBoltChange( bool value )
	{
		if( value )
			onFingerDown = OnInput_StraightBolt;
		else
			onFingerDown = ExtensionMethods.EmptyMethod;
	}

	public void OnFallDownPointChange( float value )
	{
		point_fallDown = value;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}