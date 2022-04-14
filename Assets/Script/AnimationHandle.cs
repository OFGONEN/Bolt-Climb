/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;

public class AnimationHandle : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] AnimationData[] animationDatas;
    [ SerializeField ] AnimationData animation_default;
    [ SerializeField ] Transform transform_animation;
    [ SerializeField ] ColorSetter colorSetter_animation;

// Private
    RecycledSequence animation_sequence_scale = new RecycledSequence();
    RecycledSequence animation_sequence_color = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void PlayAnimation( float progress )
    {
        for( var i = 0; i < animationDatas.Length; i++ )
        {
            if( progress <  animationDatas[ i ].data_percentage / 100f )
            {
				PlayAnimation( i );
				return;
			}
        }

		ReturnDefault();
	}
#endregion

#region Implementation
    void PlayAnimation( int index )
    {
		var data = animationDatas[ index ];

		var animation_scale = animation_sequence_scale.Recycle();
		var animation_color = animation_sequence_color.Recycle();

		animation_scale.Append( transform_animation.DOScale( data.data_scale_out, data.data_scale_duration_out ).SetEase( data.data_scale_ease_out ) );
		animation_scale.Append( transform_animation.DOScale( data.data_scale_in, data.data_scale_duration_in ).SetEase( data.data_scale_ease_in ) );
		animation_scale.SetLoops( -1 );

		animation_color.Append( DOTween.To( GetColor, SetColor, data.data_color_out, data.data_color_duration_out ).SetEase( data.data_color_ease_out ) );
		animation_color.Append( DOTween.To( GetColor, SetColor, data.data_color_in, data.data_color_duration_in ).SetEase( data.data_color_ease_in ) );
		animation_color.SetLoops( -1 );
	}

    void ReturnDefault()
    {
		var sequence = animation_sequence_scale.Recycle();
		animation_sequence_color.Kill();

		sequence.Append( transform_animation.DOScale( animation_default.data_scale_out, animation_default.data_scale_duration_out ).SetEase( animation_default.data_scale_ease_out ) );
		sequence.Join( DOTween.To( GetColor, SetColor, animation_default.data_color_out, animation_default.data_color_duration_out ).SetEase( animation_default.data_color_ease_out ) );
	}

    Color GetColor()
    {
		return colorSetter_animation.Color;
	}

    void SetColor( Color color )
    {
		colorSetter_animation.SetColor( color );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnValidate()
    {
        if( animationDatas != null && animationDatas.Length > 0 )
		    animationDatas = animationDatas.OrderBy( ( animData ) => animData.data_percentage ).ToArray();
	}
#endif
#endregion
}
