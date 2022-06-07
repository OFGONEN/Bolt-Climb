/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;

public class UICurrency : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] SharedReferenceNotifier notif_nut_reference;
    [ SerializeField ] UICurrencyPool pool_ui_currency;
    [ SerializeField ] TextMeshProUGUI text_currency;

    [ BoxGroup( "Movement" ), SerializeField ] float spawn_duration;
    [ BoxGroup( "Movement" ), SerializeField ] float spawn_depth;
    [ BoxGroup( "Movement" ), SerializeField ] float spawn_random_lateral;
    [ BoxGroup( "Movement" ), SerializeField ] float spawn_random_height;
    [ BoxGroup( "Movement" ), SerializeField ] Ease spawn_movement_ease;
    [ BoxGroup( "Scale" ), SerializeField ] Vector2 random_size_start;
    [ BoxGroup( "Scale" ), SerializeField ] Vector2 random_size_end;
    [ BoxGroup( "Scale" ), SerializeField ] Ease spawn_scale_ease;
    [ BoxGroup( "Fade" ), SerializeField ] float fade_duration;
    [ BoxGroup( "Fade" ), SerializeField ] Ease spawn_fade_ease;
// Private
	Transform nut_transform;
	RecycledSequence recycledSequence = new RecycledSequence();

	Color color_start;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnDisable()
	{
		recycledSequence.Kill();
	}

	private void OnEnable()
	{
		color_start = text_currency.color;
	}
#endregion

#region API
	public void Spawn( string value )
	{
		text_currency.text = value;
		Spawn();
	}

	public void Spawn( string value, Color color, Vector2 size )
	{
		text_currency.text = value;
		Spawn( color, size );
	}

	[ Button() ]
	public void Spawn()
	{
		var nutTransform  = notif_nut_reference.SharedValue as Transform;

		if( !nutTransform )
		{
			OnSpawnComplete();
			return;
		}

		var nutPosition   = nutTransform.position;
		    nutPosition.z = spawn_depth;

		var random = new Vector3(
			Random.Range( -spawn_random_lateral, spawn_random_lateral ),
			Random.Range( 0, spawn_random_height ),
			0
		);

		gameObject.SetActive( true );
		transform.position = nutPosition + random;
		transform.localScale = Vector3.one * random_size_start.ReturnRandom();

		text_currency.color = text_currency.color.SetAlpha( 1 );

		var sequence = recycledSequence.Recycle();

		sequence.Append( transform.DOMove( nutPosition, spawn_duration ).SetEase( spawn_movement_ease ) );
		sequence.Join( transform.DOScale( Vector3.one * random_size_end.ReturnRandom(), spawn_duration / 2f ).SetEase( spawn_scale_ease ) );
		sequence.Append( transform.DOScale( Vector3.one , spawn_duration / 2f ).SetEase( spawn_scale_ease ) );
		sequence.Append( text_currency.DOFade( 0, fade_duration ).SetEase( spawn_fade_ease ) );

		sequence.OnComplete( OnSpawnComplete );
	}

	public void Spawn( Color color, Vector2 size, float heightModifier = 1 )
	{
		var nutTransform  = notif_nut_reference.SharedValue as Transform;

		if( !nutTransform )
		{
			OnSpawnComplete();
			return;
		}

		var nutPosition   = nutTransform.position;
		    nutPosition.z = spawn_depth;

		var random = new Vector3(
			Random.Range( -spawn_random_lateral, spawn_random_lateral ),
			Random.Range( 0, spawn_random_height * heightModifier ),
			0
		);

		gameObject.SetActive( true );
		transform.position = nutPosition + random;
		transform.localScale = Vector3.one * size.ReturnRandom();

		text_currency.color = color;

		var sequence = recycledSequence.Recycle();

		sequence.Append( transform.DOMove( nutPosition, spawn_duration ).SetEase( spawn_movement_ease ) );
		// sequence.Join( transform.DOScale( Vector3.one * random_size_end.ReturnRandom(), spawn_duration / 2f ).SetEase( spawn_scale_ease ) );
		// sequence.Append( transform.DOScale( Vector3.one , spawn_duration / 2f ).SetEase( spawn_scale_ease ) );
		sequence.Append( text_currency.DOFade( 0, fade_duration ).SetEase( spawn_fade_ease ) );

		sequence.OnComplete( OnSpawnComplete );
	}
#endregion

#region Implementation
	void OnSpawnComplete()
	{
		text_currency.color = color_start;
		recycledSequence.Kill();
		pool_ui_currency.ReturnEntity( this );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}