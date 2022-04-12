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

  [ Title( "Setup" ) ]
    [ SerializeField ] Transform transform_gfx;
    [ SerializeField ] BoxCollider collider_upper_out;
    [ SerializeField ] BoxCollider collider_upper_in;
    [ SerializeField ] BoxCollider collider_bottom;

    [ SerializeField, ReadOnly, FoldoutGroup( "Info" ) ] SkinnedMeshRenderer[] bolt_renderers;
    [ ShowInInspector, ReadOnly, ProgressBar( 0, 1 ), FoldoutGroup( "Info" ) ] float bolt_carve_progress;

	// Private
	Transform transform_nut;
	float point_bottom;
	float point_up;

    // Delegate
    UnityMessage onStartTrackingNut;
    UnityMessage onUpdateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		point_bottom = transform.position.y;
		point_up     = collider_upper_out.transform.position.y + collider_upper_out.size.y / 2f;

		onStartTrackingNut = StartTrackingNut;
		onUpdateMethod     = ExtensionMethods.EmptyMethod;
	}
#endregion

#region API
    public void OnStartTrackingNut()
    {
		onStartTrackingNut();
	}

    public void OnStopTrackingNut()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Implementation
    void StartTrackingNut()
    {
		onStartTrackingNut = ExtensionMethods.EmptyMethod;
		onUpdateMethod     = OnTrackNut;

		notifier_nut_fallDown.SharedValue = transform.position.y;
		transform_nut                     = notifier_nut_reference.SharedValue as Transform;
	}

    void OnTrackNut()
    {
		bolt_carve_progress = Mathf.Max( bolt_carve_progress, ( point_up - transform_nut.position.y ) / ( point_up - point_bottom ) );
		UpdateCarveProgress();
	}

    void UpdateCarveProgress()
    {
		var step      = 1f / bolt_renderers.Length;
		var stepCount = bolt_carve_progress / step;

		var childCount = Mathf.FloorToInt( stepCount );
		var remaining = stepCount - childCount;

		for( var i = 0; i < childCount; i++ )
        {
			bolt_renderers[ i ].SetBlendShapeWeight( 0, 100 );
		}

        if( !Mathf.Approximately( remaining, 0 ) )
            bolt_renderers[ childCount ].SetBlendShapeWeight( 0, remaining * 100 );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    [ ShowInInspector, BoxGroup( "EditorOnly" ) ] private GameObject bolt_prefab;
    [ ShowInInspector, BoxGroup( "EditorOnly" ) ] private int bolt_count;
    [ ShowInInspector, BoxGroup( "EditorOnly" ) ] private float bolt_height;

    private void CacheRenderers()
    {
		bolt_renderers = transform.GetComponentsInChildren< SkinnedMeshRenderer >();
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