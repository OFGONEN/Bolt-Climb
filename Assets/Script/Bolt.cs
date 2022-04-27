/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEditor;
using Sirenix.OdinInspector;

public class Bolt : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] SharedReferenceNotifier notifier_nut_reference;
    [ SerializeField ] SharedFloatNotifier notifier_nut_fallDown;
    [ SerializeField ] Currency property_currency;

  [ Title( "Setup" ) ]
    [ SerializeField ] Transform transform_gfx;
    [ SerializeField ] BoxCollider collider_upper_out;
    [ SerializeField ] BoxCollider collider_upper_in;
    [ SerializeField ] BoxCollider collider_bottom;
    [ SerializeField ] ParticleSystem particle_nut_carving;

    [ SerializeField, ReadOnly, FoldoutGroup( "Info" ) ] SkinnedMeshRenderer[] bolt_renderers;
    [ ShowInInspector, ReadOnly, ProgressBar( 0, 1 ), FoldoutGroup( "Info" ) ] float bolt_carve_progress;

	// Private
	Transform transform_nut;
	float point_bottom;
	float point_up;
	float point_gap;

	// Delegate
	UnityMessage onStartTrackingNut;
    UnityMessage onUpdateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		point_bottom        = transform.position.y;
		point_up            = collider_upper_out.transform.position.y + collider_upper_out.size.y / 2f;
		point_gap           = point_up - point_bottom;
		bolt_carve_progress = -1f;

		onStartTrackingNut = StartTrackingNut;
		onUpdateMethod     = ExtensionMethods.EmptyMethod;
	}

	private void Update()
	{
		onUpdateMethod();
	}
#endregion

#region API
    public void OnStartTrackingNut()
    {
		onStartTrackingNut();
	}

    public void OnStopTrackingNut()
    {
		particle_nut_carving.Stop( true, ParticleSystemStopBehavior.StopEmitting );
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Implementation
    void StartTrackingNut()
    {
		FFLogger.Log( "Start Tracking Nut", gameObject );
		onStartTrackingNut = ExtensionMethods.EmptyMethod;
		onUpdateMethod     = OnTrackNut;

		notifier_nut_fallDown.SharedValue = transform.position.y;
		transform_nut                     = notifier_nut_reference.SharedValue as Transform;
	}

    void OnTrackNut()
    {
		var progress = bolt_carve_progress;
		bolt_carve_progress = Mathf.Clamp( ( transform_nut.position.y - point_bottom ) / point_gap, bolt_carve_progress, 1 );


		if( bolt_carve_progress > progress )
		{
			particle_nut_carving.Play( true );
			particle_nut_carving.transform.position = particle_nut_carving.transform.position.SetY( Mathf.Lerp( point_bottom, point_up, bolt_carve_progress ) );
			property_currency.OnIncrease();
		}
		else
			particle_nut_carving.Stop( true, ParticleSystemStopBehavior.StopEmitting );


		UpdateCarveProgress();
	}

    void UpdateCarveProgress()
    {
		var step     = 1f / bolt_renderers.Length;
		var progress = bolt_carve_progress;

		for( var i = 0; i < bolt_renderers.Length; i++ )
        {
			if( progress >= step )
			{
				bolt_renderers[ i ].SetBlendShapeWeight( 0, 100 );
				progress -= step;
			}
			else
			{
				bolt_renderers[ i ].SetBlendShapeWeight( 0, progress / step * 100 );
				progress = Mathf.Max( progress - step, 0 );
			}
		}
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    [ ShowInInspector, BoxGroup( "EditorOnly" ), AssetSelector( Paths = "Assets/Prefab/GFX"  ) ] private GameObject bolt_prefab;
    [ ShowInInspector, BoxGroup( "EditorOnly" ) ] public int bolt_count;
    [ ShowInInspector, BoxGroup( "EditorOnly" ) ] public float bolt_height = 0.5f;

    private void CacheRenderers()
    {
		bolt_renderers = transform.GetComponentsInChildren< SkinnedMeshRenderer >();
	}

    [ Button() ]
    public void PlaceBolts( int count, float height, GameObject prefab )
    {
		bolt_count  = count;
		bolt_height = height;
		bolt_prefab = prefab;

		PlaceBolts();
	}

    [ Button() ]
    private void PlaceBolts()
    {
		transform_gfx.DestoryAllChildren();

		for( var i = 0; i < bolt_count; i++ )
        {
		    var prefab = PrefabUtility.InstantiatePrefab( bolt_prefab, transform_gfx ) as GameObject;
			var position = transform_gfx.localPosition;
			position.y += bolt_height * i;
			prefab.transform.localPosition = position;
			prefab.name = prefab.name + "_" + ( i + 1 );
		}

		collider_bottom.transform.localPosition    = Vector3.up * -1f * collider_bottom.size.y / 2f;
		collider_upper_out.transform.localPosition = Vector3.up * bolt_count * bolt_height - Vector3.up * collider_bottom.size.y / 2f;
		collider_upper_in.transform.localPosition  = Vector3.up * bolt_count * bolt_height + Vector3.up * collider_bottom.size.y / 2f;
		CacheRenderers();
	}

    // [ Button() ]
    // private void TestCarveProgress( float progress )
    // {
    //     for( var i = 0; i < bolt_renderers.Length; i++ )
    //     {
	// 		bolt_renderers[ i ].SetBlendShapeWeight( 0, 0 );
	// 	}

	// 	bolt_carve_progress = progress;
	// 	UpdateCarveProgress();
	// }
#endif
#endregion
}