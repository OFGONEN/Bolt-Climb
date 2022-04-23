/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class DissolveSetter : MonoBehaviour
{
#region Fields
    [ SerializeField ] Renderer[] bolt_renderers;
	[ SerializeField ] Vector2 bolt_dissolve_range;

// Private
	float bolt_dissolve_progress;

	static int SHADER_ID_COLOR = Shader.PropertyToID( "_threshold" );
	MaterialPropertyBlock propertyBlock;

	UnityMessage onProgressUpdate;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		propertyBlock          = new MaterialPropertyBlock();
		bolt_dissolve_progress = 0;

		onProgressUpdate = ExtensionMethods.EmptyMethod;
	}
#endregion

#region API
	public void StartTracking()
	{
		onProgressUpdate = UpdateDissolveProgress;
	}

	public void StopTracking()
	{
		onProgressUpdate = ExtensionMethods.EmptyMethod;
	}

	public void UpdateProgress( float progress )
	{
		bolt_dissolve_progress = progress;
		onProgressUpdate();
	}
#endregion

#region Implementation
    void UpdateDissolveProgress()
    {
		var step     = 1f / bolt_renderers.Length;
		var progress = bolt_dissolve_progress;

		for( var i = 0; i < bolt_renderers.Length; i++ )
        {
			if( progress >= step )
			{
				SetDissolve( bolt_renderers[ i ], bolt_dissolve_range.x );
				progress -= step;
			}
			else
			{
				SetDissolve( bolt_renderers[ i ], bolt_dissolve_range.ReturnProgressInverse( progress / step ) );
				progress = Mathf.Max( progress - step, 0 );
			}
		}
	}

	void SetDissolve( Renderer renderer, float value )
	{
		renderer.GetPropertyBlock( propertyBlock );
		propertyBlock.SetFloat( SHADER_ID_COLOR, value );
		renderer.SetPropertyBlock( propertyBlock );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}