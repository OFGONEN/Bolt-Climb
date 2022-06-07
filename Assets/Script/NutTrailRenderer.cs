/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;

public class NutTrailRenderer : MonoBehaviour
{
#region Fields
    [ SerializeField ] Mesh[] meshes;

    [ SerializeField ] ParticleSystemRenderer particleSystemRenderer;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    [ Button() ]
	public void SetMesh( int index )
	{
		particleSystemRenderer.mesh = meshes[ index ];
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}