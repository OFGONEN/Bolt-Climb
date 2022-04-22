/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "animation_data", menuName = "FF/Data/Game/Animation" ) ]
public class AnimationData : ScriptableObject
{
#region Fields
    public float data_percentage;
  [ Title( "Scale Data" ) ]
// Out Scale
    public float data_scale_out; 
    public float data_scale_duration_out; 
    public Ease  data_scale_ease_out; 
// In Scale
    public float data_scale_in; 
    public float data_scale_duration_in; 
    public Ease  data_scale_ease_in; 
  [ Title( "Color Data" ) ]
// Out Color
    public Color data_color_out = Color.white; 
    public float data_color_duration_out; 
    public Ease  data_color_ease_out; 
// In Color
    public Color data_color_in = Color.white; 
    public float data_color_duration_in; 
    public Ease  data_color_ease_in; 
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnValidate()
	{
		data_color_out = data_color_out.SetAlpha( 1 );
		data_color_in  = data_color_in.SetAlpha( 1 );
	}
#endif
#endregion
}
