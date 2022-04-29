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
	[ SerializeField ] RustSetter component_rust_setter;
	[ SerializeField ] ParticleSystem particle_carving;


  [ Title( "Particle" )]
	[ SerializeField ] ParticleSystem particle_nut_lowDurability;
// Private
	float point_fallDown = 0;
	float point_levelEnd;
	bool onPath;
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
	public void OnLevel_Failed()
	{
		EmptyDelegates();
	}

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
			FFLogger.Log( "Nut Exit Bolt" );

			onFingerDown   = ExtensionMethods.EmptyMethod;
			onFingerUp     = ExtensionMethods.EmptyMethod;

			if( onPath )
				onUpdateMethod = ExtensionMethods.EmptyMethod;
			else
				onUpdateMethod = OnUpdate_Deceleration;
		}
	}

	public void OnFallDownPointChange( float value )
	{
		point_fallDown = value;
	}

	public void OnShapedBolt( IntGameEvent gameEvent )
	{
		FFLogger.Log( "Start Shaped Bolt" );

		particle_carving.Play( true );

		onPath = true;
		EmptyDelegates();
		component_movement.DoPath( gameEvent.eventValue, OnPathComplete );
	}

	public void OnLevelEndBolt( IntGameEvent gameEvent )
	{
		FFLogger.Log( "End Bolt" );

		onPath = true;
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
		FFLogger.Log( "On Shaped Path Complete" );
		onPath = false;

		particle_carving.Stop( true, ParticleSystemStopBehavior.StopEmitting );

		var position   = transform.position;
		    position.x = 0;
		    position.z = 0;

		transform.position = position; // todo remove this after path points are corrected

		property_velocity.SetMinimumVelocity();
		onUpdateMethod = OnUpdate_Deceleration;
	}

	void OnLevelEndPathComplete()
	{
		onPath = false;
		component_rigidbody.isKinematic = false;
		// component_rigidbody.useGravity  = true;
		component_collider.isTrigger    = false;

		var force = GameSettings.Instance.nut_levelEnd_force.ReturnClamped( property_velocity.CurrentVelocity );

		component_rigidbody.AddForce( Vector3.forward * force, ForceMode.Impulse );
		component_rigidbody.AddTorque( Random.onUnitSphere * force, ForceMode.Impulse );

		event_path_end.Raise();

		DOVirtual.DelayedCall( GameSettings.Instance.nut_levelEnd_waitDuration, event_level_completed.Raise );
	}

	void OnUpdate_Idle()
	{
		property_durability.OnIncrease();
		var animationProgress = component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );
		component_rust_setter.SetRust( animationProgress );
	}

	void OnUpdate_Acceleration()
	{
		if( Mathf.Approximately( 0, property_durability.CurrentDurability ) )
		{
			EmptyDelegates();
			gameObject.SetActive( false );

			var shatter                    = pool_randomShatter.GetEntity();
			    shatter.transform.position = transform.position;

			shatter.DoShatter( component_rust_setter.Rust );

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
			var animationProgress = component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );
			component_rust_setter.SetRust( animationProgress );
		}
	}

	void OnUpdate_Deceleration()
	{
		property_velocity.OnDeceleration();
		var isIdle = component_movement.OnMovement( point_fallDown );

		property_durability.OnIncrease();
		var animationProgress = component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );
		component_rust_setter.SetRust( animationProgress );

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