/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;

public class NutTrailRenderer : MonoBehaviour
{
#region Fields
	[ SerializeField ] SkillData skill_lastChance_doubleJump;

    [ SerializeField ] Mesh[] meshes;
    [ SerializeField ] ParticleSystem particleSystem_nutTrail;
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

	public void OnNutOnBoltChange( bool value )
	{
		if( value )
			Deactivate();
		else if( skill_lastChance_doubleJump.IsUnlocked )
			Activate();
	}
	
	[ Button() ]
	public void Activate()
	{
		particleSystem_nutTrail.Play();
	}
	
	[ Button() ]
	public void Deactivate()
	{
		particleSystem_nutTrail.Stop( false, ParticleSystemStopBehavior.StopEmitting );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}