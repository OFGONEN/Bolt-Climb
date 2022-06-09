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
	[ SerializeField ] GameEvent event_nut_EndLevel;
	[ SerializeField ] GameEvent event_curvedPath_end;
	[ SerializeField ] GameEvent event_path_end;
	[ SerializeField ] GameEvent event_nut_air_update;
	[ SerializeField ] GameEvent event_durability_deplated;
	[ SerializeField ] SharedFloatNotifier level_progress;
	[ SerializeField ] SharedFloatNotifier notif_nut_height;
	[ SerializeField ] SharedFloatNotifier notif_nut_height_last;
	[ SerializeField ] SkinLibrary library_skin;

  [ Title( "Components" )]
	[ SerializeField ] Movement component_movement;
	[ SerializeField ] AnimationHandle component_animation;
	[ SerializeField ] Velocity property_velocity;
	[ SerializeField ] Durability property_durability;
	[ SerializeField ] Currency property_currency;
	[ SerializeField ] Rigidbody component_rigidbody;
	[ SerializeField ] Collider component_collider;
	[ SerializeField ] CrackSetter component_crack_setter;
	[ SerializeField ] ParticleSystem particle_carving;
	[ SerializeField ] MeshFilter component_mesh_filter;
	[ SerializeField ] MeshRenderer component_mesh_renderer;
	[ SerializeField ] NutTrailRenderer component_trail_renderer;

  [ Title( "Particle" )]
	[ SerializeField ] ParticleSystem particle_nut_lowDurability;
// Private
	float point_fallDown = 0;
	float point_levelEnd;
	bool onPath;

	Color crackColor;
// Delegates
	UnityMessage onUpdateMethod;
	UnityMessage onFingerDown;
	UnityMessage onFingerUp;
	UnityMessage onLevelProgress;
	UnityMessage onUpdate_Air;
	UnityMessage_Bool onNut_IsOnBoltChange;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnDisable()
	{
		EmptyDelegates();
		onLevelProgress = ExtensionMethods.EmptyMethod;
	}

	private void Awake()
	{
		onUpdateMethod       = ExtensionMethods.EmptyMethod;
		onFingerDown         = ExtensionMethods.EmptyMethod;
		onFingerUp           = ExtensionMethods.EmptyMethod;
		onUpdate_Air         = ExtensionMethods.EmptyMethod;
		onNut_IsOnBoltChange = NutOnBoltChange;

		onLevelProgress = UpdateLevelProgress;

		OnSkin_Changed();
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
	[ Button() ]
	public void OnSkin_Changed()
	{
		var skinIndex = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.nut_skin_index, 0 );
		var mesh = library_skin.GetMesh( skinIndex );
		crackColor = library_skin.GetCrackColor( skinIndex );

		component_mesh_filter.mesh             = library_skin.GetMesh( skinIndex );
		component_mesh_renderer.sharedMaterial = library_skin.GetMaterial( skinIndex );
		component_crack_setter.Setup( crackColor );
		component_trail_renderer.SetMesh( mesh );
	}

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
		onNut_IsOnBoltChange( value );
	}

	public void OnFallDownPointChange( float value )
	{
		point_fallDown = value;
	}

	public void OnShapedBolt( IntGameEvent gameEvent )
	{
		particle_carving.Play( true );

		onPath = true;
		EmptyDelegates();
		property_velocity.SetMinimumPathVelocity();
		component_movement.DoPath( gameEvent.eventValue, OnPathComplete );

		onNut_IsOnBoltChange = NutOnBoltChange;
	}

	public void OnLevelEndBolt( IntGameEvent gameEvent )
	{
		onPath = true;
		EmptyDelegates();
		onLevelProgress = ExtensionMethods.EmptyMethod;
		property_velocity.SetMinimumPathVelocity();
		component_movement.DoPathEnd( gameEvent.eventValue, OnLevelEndPathComplete );

		notif_nut_height_last.SharedValue = 0;
		PlayerPrefsUtility.Instance.SetFloat( ExtensionMethods.nut_height, 0 );
	}

	public void OnShatter()
	{
		EmptyDelegates();
		gameObject.SetActive( false );

		var shatter = pool_randomShatter.GetEntity();
		shatter.transform.position = transform.position;

		shatter.DoShatter( component_crack_setter.Fragility, crackColor );

		var height = transform.position.y;
		notif_nut_height_last.SharedValue = height;
		PlayerPrefsUtility.Instance.SetFloat( ExtensionMethods.nut_height, height );

		DOVirtual.DelayedCall( GameSettings.Instance.nut_shatter_waitDuration, event_level_failed.Raise );
	}
#endregion

#region Implementation
	void NutOnBoltChange( bool value )
	{
		if( value )
		{
			onUpdate_Air = ExtensionMethods.EmptyMethod;
			onFingerDown = OnFingerDown_StraightBolt;
		}
		else
		{
			onUpdate_Air = event_nut_air_update.Raise;
			onFingerDown = ExtensionMethods.EmptyMethod;
			onFingerUp   = ExtensionMethods.EmptyMethod;

			if( onPath )
				onUpdateMethod = ExtensionMethods.EmptyMethod;
			else
				onUpdateMethod = OnUpdate_Deceleration;
		}
	}

	void OnPathComplete()
	{
		onPath = false;
		event_curvedPath_end.Raise();

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

		DOVirtual.DelayedCall( GameSettings.Instance.nut_levelEnd_waitDuration, event_nut_EndLevel.Raise );
	}

	void OnUpdate_Idle()
	{
		property_durability.OnIncrease();
		component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );
		component_crack_setter.SetFragility( 1 - property_durability.DurabilityRatio );
	}

	void OnUpdate_Acceleration()
	{
		if( Mathf.Approximately( 0, property_durability.CurrentDurability ) )
		{
			EmptyDelegates();
			onUpdateMethod = OnUpdate_LastChance;
			event_durability_deplated.Raise();
		}
		else
		{
			property_velocity.OnAcceleration();
			component_movement.OnMovement();
			property_durability.OnDecrease();
			component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );
			component_crack_setter.SetFragility( 1 - property_durability.DurabilityRatio );
		}
	}

	void OnUpdate_LastChance()
	{
		component_movement.OnMovement();
		component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );
	}

	void OnUpdate_Deceleration()
	{
		onUpdate_Air();
		property_velocity.OnDeceleration();
		var isIdle = component_movement.OnMovement( point_fallDown );

		property_durability.OnIncrease();
		component_animation.PlayAnimation( property_durability.DurabilityRatio, particle_nut_lowDurability );
		component_crack_setter.SetFragility( 1 - property_durability.DurabilityRatio );

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
		onUpdateMethod       = ExtensionMethods.EmptyMethod;
		onFingerDown         = ExtensionMethods.EmptyMethod;
		onFingerUp           = ExtensionMethods.EmptyMethod;
		onUpdate_Air         = ExtensionMethods.EmptyMethod;
		onNut_IsOnBoltChange = ExtensionMethods.EmptyMethod;
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
#endif
#endregion
}