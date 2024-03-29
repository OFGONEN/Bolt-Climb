/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class CameraController : MonoBehaviour
{
#region Fields
    [ SerializeField ] SharedReferenceNotifier notif_target_transform;
    [ SerializeField ] Velocity target_velocity;

// Private
    [ ShowInInspector, ReadOnly ] Transform target_transform;
    UnityMessage onUpdateMethod;
	Vector3 target_offset;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

    private void OnDisable()
    {
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		onUpdateMethod();
	}
#endregion

#region API
    public void OnLevelStart()
    {
		target_transform = notif_target_transform.SharedValue as Transform;
		onUpdateMethod   = FollowTarget;
	}

    public void OnLevelEndPath_Start()
    {
		target_offset  = target_transform.InverseTransformPoint( transform.position );
		onUpdateMethod = FollowTargetWithOffset;
	}

    public void OnLevelEndPath_End()
    {
		onUpdateMethod = LookAtTarget;
    }
#endregion

#region Implementation
    void FollowTarget()
    {
		var position   = transform.position;
		    position.y = Mathf.Lerp( position.y, target_transform.position.y, Time.deltaTime * Mathf.Abs( target_velocity.CurrentVelocity ) );

		transform.position = position;
	}

	void FollowTargetWithOffset()
	{
		var targetPosition = target_transform.TransformPoint( target_offset );
		var position = transform.position;
		position.y = Mathf.Lerp( position.y, targetPosition.y, Time.deltaTime * Mathf.Abs( target_velocity.CurrentVelocity ) );
		position.z = Mathf.Lerp( position.z, targetPosition.z, Time.deltaTime * Mathf.Abs( target_velocity.CurrentVelocity ) );

		transform.position = position;
		transform.LookAtAxis( target_transform.position, new Vector3( 1, 1, 0 ) );
	}

	void LookAtTarget()
	{
		transform.LookAtAxis( target_transform.position, new Vector3( 1, 0, 0 ) );
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}