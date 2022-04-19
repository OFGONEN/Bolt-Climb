/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class IncrementalButton : UIEntity
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] Button ui_button;

// Private
    bool available = false;
#endregion

#region Properties
    public bool Availability => available;
#endregion

#region Unity API
    private void Awake()
    {
		ui_button.interactable = false;
		available              = false;
	}
#endregion

#region API
    public void OnLevelLoaded()
    {
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
