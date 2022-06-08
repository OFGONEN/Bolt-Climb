/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class CrackSetter : MonoBehaviour
{
#region Fields
    [ SerializeField ] Renderer _renderer;

	static int SHADER_ID_FRAGILITY   = Shader.PropertyToID( "_Fragility" );
	static int SHADER_ID_COLOR_CRACK = Shader.PropertyToID( "_Crack_Color" );
	MaterialPropertyBlock propertyBlock;

	float fragility;
#endregion

#region Properties
	public float Fragility => fragility;
#endregion

#region Unity API
    private void Awake()
    {
		propertyBlock = new MaterialPropertyBlock();
		SetFragility( 0 );
	}
#endregion

#region API
    [ Button() ]
	public void SetFragility( float fragility )
	{
		this.fragility = GameSettings.Instance.shader_range_crack.ReturnProgress( fragility );
		_renderer.GetPropertyBlock( propertyBlock );
		propertyBlock.SetFloat( SHADER_ID_FRAGILITY, fragility );
		_renderer.SetPropertyBlock( propertyBlock );
	}
	
    [ Button() ]
	public void SetCrackColor( Color color )
	{
		_renderer.GetPropertyBlock( propertyBlock );
		propertyBlock.SetColor( SHADER_ID_COLOR_CRACK, color );
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
