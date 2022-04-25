/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class RustSetter : MonoBehaviour
{
#region Fields
    [ SerializeField ] Renderer _renderer;

// Private
	static int SHADER_ID_COLOR = Shader.PropertyToID( "_threshold" );
	MaterialPropertyBlock propertyBlock;

	float rust_progress;
#endregion

#region Properties
	public float Rust => rust_progress;
#endregion

#region Unity API
    private void Awake()
    {
		propertyBlock = new MaterialPropertyBlock();
		SetRust( 0 );
	}
#endregion

#region API
    [ Button() ]
	public void SetRust( float value )
	{
		rust_progress = GameSettings.Instance.shader_range_rust.ReturnProgress( value );
		_renderer.GetPropertyBlock( propertyBlock );
		propertyBlock.SetFloat( SHADER_ID_COLOR, rust_progress );
		_renderer.SetPropertyBlock( propertyBlock );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnValidate()
	{
		var renderer = GetComponent< Renderer >();

		if( renderer && !_renderer )
			_renderer = renderer;
	}
#endif
#endregion
}
