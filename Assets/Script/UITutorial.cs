/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FFStudio;
using Sirenix.OdinInspector;
using DG.Tweening;

public class UITutorial : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] Image ui_image;
    [ SerializeField ] TextMeshProUGUI ui_text;
    [ SerializeField ] SharedReferenceNotifier ui_target;

  [ Title( "Settings" ) ]
    [ SerializeField ] float sequence_duration;
    [ SerializeField ] float sequence_duration_end;
    [ SerializeField ] float image_size_cofactor_start;
    [ SerializeField ] float image_size_cofactor_end;
    [ SerializeField ] float shake_duration;

    UnityMessage onInputMethod;
    RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		onInputMethod = ExtensionMethods.EmptyMethod;
		CleanUp();
	}
#endregion

#region API
    [ Button() ]
    public void StartTutorial()
    {
		Time.timeScale = GameSettings.Instance.game_tutorial_timeScale;
		onInputMethod  = FinishTutorial;

		var position       = ( ui_target.SharedValue as Transform ).position;
		var screenPosition = Camera.main.WorldToScreenPoint( position );

		ui_image.enabled = true;
		ui_text.enabled  = true;

		var image_size_start = Screen.width / image_size_cofactor_start;
		var image_size_end   = Screen.width / image_size_cofactor_end;

		ui_text.rectTransform.position   = screenPosition;
		ui_image.rectTransform.position  = screenPosition;
		ui_image.rectTransform.sizeDelta = new Vector2( image_size_start, image_size_start );

		var sequence           = recycledSequence.Recycle( OnSequenceComplete );

		sequence.Append( ui_image.rectTransform.DOSizeDelta( new Vector2( image_size_end, image_size_end ), sequence_duration ).SetEase( Ease.Linear ) )
				.Join( ui_image.DOColor( Color.red, sequence_duration ) )
				.Join( ui_text.rectTransform.DOShakeScale( shake_duration, 1, 10, 90, false ).SetLoops( int.MaxValue, LoopType.Yoyo ) );

		sequence.SetUpdate( true );
	}

    public void OnInput()
    {
		onInputMethod();
	}
#endregion

#region Implementation
    void OnSequenceComplete()
    {
		Time.timeScale = 1f;
		onInputMethod = ExtensionMethods.EmptyMethod;

		ui_image.color = Color.red;

		var sequence = recycledSequence.Recycle( CleanUp );

		sequence.Append( ui_text.DOFade( 0, sequence_duration_end ) )
				.Append( ui_image.DOFade( 0, sequence_duration_end ) );
    }

    [ Button() ]
    void FinishTutorial()
    {
		Time.timeScale = 1f;
		onInputMethod = ExtensionMethods.EmptyMethod;

		ui_image.color = Color.green;

		var sequence = recycledSequence.Recycle( CleanUp );

		sequence.Append( ui_text.DOFade( 0, sequence_duration_end ) )
				.Append( ui_image.DOFade( 0, sequence_duration_end ) );
	}

    void CleanUp()
    {
		ui_image.color = Color.white;
		ui_text.color  = Color.white;

		ui_image.enabled = false;
		ui_text.enabled  = false;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}