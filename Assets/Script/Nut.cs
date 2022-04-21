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
	[ SerializeField ] SharedReferenceNotifier notif_bolt_end;
	[ SerializeField ] GameEvent event_level_failed;
	[ SerializeField ] GameEvent event_level_completed;
	[ SerializeField ] GameEvent event_path_end;
	[ SerializeField ] SharedFloatNotifier level_progress;
	[ SerializeField ] SharedFloatNotifier notif_nut_height;
	[ SerializeField ] SharedFloatNotifier notif_nut_height_last;

  [ Title( "Components" )]
	[ SerializeField ] Movement component_movement;
	[ SerializeField ] AnimationHandle component_animation;
	[ SerializeField ] Velocity property_velocity;
	[ SerializeField ] Durability property_durability;
	[ SerializeField ] Currency property_currency;
	[ SerializeField ] Rigidbody component_rigidbody;
	[ SerializeField ] Collider component_collider;


  [ Title( "Components" )]
	[ SerializeField ] ParticleSystem particle_nut_lowDurability;
	[ SerializeField ] ParticleSystem particle_nut_carving;
// Private
	float point_fallDown = 0;
	float point_levelEnd;
// Delegates
	UnityMessage onUpdateMethod;
	UnityMessage onFingerDown;
	UnityMessage onFingerUp;
	UnityMessage onLevelProgress;
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		onUpdateMethod  = ExtensionMethods.EmptyMethod;
		onFingerDown    = ExtensionMethods.EmptyMethod;
		onFingerUp      = ExtensionMethods.EmptyMethod;
		onLevelProgress = UpdateLevelProgress;
	}

	private void Start()
	{
		point_levelEnd = ( notif_bolt_end.SharedValue as Transform ).position.y;
		UpdateLevelProgress();
	}

	private void Update()
	{
		onUpdateMethod();
		onLevelProgress();
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
		{
			onFingerDown   = ExtensionMethods.EmptyMethod;
			onFingerUp     = ExtensionMethods.EmptyMethod;
			onUpdateMethod = OnUpdate_Deceleration;

			particle_nut_carving.Stop();
		}
	}

	public void OnFallDownPointChange( float value )
	{
		point_fallDown = value;
	}

	public void OnShapedBolt( IntGameEvent gameEvent )
	{
		EmptyDelegates();
		component_movement.DoPath( gameEvent.eventValue, OnPathComplete );
	}

	public void OnLevelEndBolt( IntGameEvent gameEvent )
	{
		EmptyDelegates();
		onLevelProgress = ExtensionMethods.EmptyMethod;
		component_movement.DoPath( gameEvent.eventValue, OnLevelEndPathComplete );

		notif_nut_height_last.SharedValue = 0;
		PlayerPrefs.SetFloat( ExtensionMethods.nut_height, 0 );
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
		component_rigidbody.isKinematic = false;
		// component_rigidbody.useGravity  = true;
		component_collider.isTrigger    = false;

		component_rigidbody.AddForce( Vector3.forward * property_velocity.CurrentVelocity, ForceMode.Impulse );
		component_rigidbody.AddTorque( Random.onUnitSphere * property_velocity.CurrentVelocity * GameSettings.Instance.nut_levelEnd_torque_cofactor, ForceMode.Impulse );

		event_path_end.Raise();

		DOVirtual.DelayedCall( GameSettings.Instance.nut_levelEnd_waitDuration, event_level_completed.Raise );
	}

	void OnUpdate_Idle()
	{
		property_durability.OnIncrease();
		component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );
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

			var height = transform.position.y;
			notif_nut_height_last.SharedValue = height;
			PlayerPrefs.SetFloat( ExtensionMethods.nut_height, height );

			DOVirtual.DelayedCall( GameSettings.Instance.nut_shatter_waitDuration, event_level_failed.Raise );
		}
		else
		{
			property_velocity.OnAcceleration();
			component_movement.OnMovement();
			property_durability.OnDecrease();
			component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );
			property_currency.OnIncrease();
		}
	}

	void OnUpdate_Deceleration()
	{
		property_velocity.OnDeceleration();
		var isIdle = component_movement.OnMovement( point_fallDown );

		property_durability.OnIncrease();
		component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );

		if( isIdle )
			onUpdateMethod = OnUpdate_Idle;
	}

	void OnFingerDown_StraightBolt()
	{
		particle_nut_carving?.Play();
		onUpdateMethod = OnUpdate_Acceleration;
		onFingerUp     = OnFingerUp;
	}

	void OnFingerUp()
	{
		onUpdateMethod = OnUpdate_Deceleration;
		onFingerUp     = ExtensionMethods.EmptyMethod;

		particle_nut_carving.Stop();
	}

	void EmptyDelegates()
	{
		onUpdateMethod = ExtensionMethods.EmptyMethod;
		onFingerUp     = ExtensionMethods.EmptyMethod;
		onFingerDown   = ExtensionMethods.EmptyMethod;
	}

	void UpdateLevelProgress()
	{
		var height          = transform.position.y;
		var baseProgress    = ( CurrentLevelData.Instance.currentLevel_Shown - 1 ) / ( float )GameSettings.Instance.game_level_count;
		var currentProgress = height / point_levelEnd;

		level_progress.SharedValue   = baseProgress + currentProgress / GameSettings.Instance.game_level_count;
		notif_nut_height.SharedValue = height;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
//! todo remove this variable before build
	// [ SerializeField ] SharedBoolNotifier isNutOnBolt;

	private void OnGUI() 
	{
		var style = new GUIStyle();
		style.fontSize = 25;

		// GUI.Label( new Rect( 25, 50 , 250, 250 ), "Is Nut On Bolt: " + isNutOnBolt.SharedValue  , style);
		GUI.Label( new Rect( 25, 75 , 250, 250 ), "Nut Durability: " + property_durability.CurrentDurability , style);
		GUI.Label( new Rect( 25, 100, 250, 250 ), "Nut %Durability: " + property_durability.DurabilityRatio , style);
		GUI.Label( new Rect( 25, 125, 250, 250 ), "Nut Velocity: " + property_velocity.CurrentVelocity , style);
		GUI.Label( new Rect( 25, 150, 250, 250 ), "Curreny: " + property_currency.SharedValue , style);
		GUI.Label( new Rect( 25, 170, 250, 250 ), "Height: " + transform.position.y , style);
	}
#endif
#endregion
}