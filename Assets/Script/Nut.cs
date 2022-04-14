/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Nut : MonoBehaviour
{
#region Fields
  [ Title( "Shared Variables" ) ]
    [ SerializeField ] SharedFloatNotifier notif_nut_point_fallDown;
    [ SerializeField ] SharedBoolNotifier notif_nut_is_onBolt;

  [ Title( "Components" ) ]
    [ SerializeField ] Movement component_movement;
// Private

// Delegates
    UnityMessage onUpdate;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		onUpdate = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		onUpdate();
	}
#endregion

#region API
    public void Input_OnFingerDown()
    {
    }

    public void Input_OnFingerUp()
    {
    }
#endregion

#region Implementation
    //todo DoIdle ( Movement ? )
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}