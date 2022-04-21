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
		// target_offset  = target_transform.position - transform.position;
		// onUpdateMethod = FollowTargetWithOffset;
		onUpdateMethod = ExtensionMethods.EmptyMethod;
	}

    public void OnLevelEndPath_End()
    {
		// onUpdateMethod = LookAtTarget;
    }
#endregion

#region Implementation
    void FollowTarget()
    {
		var position   = transform.position;
		    position.y = Mathf.Lerp( position.y, target_transform.position.y, Time.deltaTime * Mathf.Abs( target_velocity.CurrentVelocity ) );

		transform.position = position;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}