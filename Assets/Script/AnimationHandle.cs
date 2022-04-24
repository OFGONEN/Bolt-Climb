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

// Private
    RecycledSequence animation_sequence_scale = new RecycledSequence();
    int animation_index = -1;
#endregion

#region Properties
#endregion

#region Unity API
	private void OnDisable()
	{
		animation_sequence_scale.Kill();
	}
#endregion

#region API
    public float PlayAnimation( float progress, ParticleSystem particle )
    {
        var index = -1;
		var initialPercentage = animationDatas[ 0 ].data_percentage / 100f;

		for( var i = 0; i < animationDatas.Length; i++ )
        {
            if( progress <=  initialPercentage )
            {
				index = i;
				break;
			}
        }

        if( index == -1 && animation_index != -1 )
		{
		    ReturnDefault();
			particle.Stop();
		}
        else if( index != animation_index )
		{
			PlayAnimation( index );
			particle.Play();
		}

		animation_index = index;

		return 1f - ( Mathf.Min( progress, initialPercentage ) / initialPercentage );
	}
#endregion

#region Implementation
    void PlayAnimation( int index )
    {
		var data = animationDatas[ index ];

		var animation_scale = animation_sequence_scale.Recycle();

		animation_scale.Append( transform_animation.DOScale( data.data_scale_out, data.data_scale_duration_out ).SetEase( data.data_scale_ease_out ) );
		animation_scale.Append( transform_animation.DOScale( data.data_scale_in, data.data_scale_duration_in ).SetEase( data.data_scale_ease_in ) );
		animation_scale.SetLoops( -1, LoopType.Yoyo );
	}

    void ReturnDefault()
    {
		var sequence = animation_sequence_scale.Recycle();

		sequence.Append( transform_animation.DOScale( animation_default.data_scale_out, animation_default.data_scale_duration_out ).SetEase( animation_default.data_scale_ease_out ) );
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