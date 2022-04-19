/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FFStudio;
using TMPro;
using Sirenix.OdinInspector;

public class IncrementalButton : UIEntity
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] Button ui_button;
    [ SerializeField ] TextMeshProUGUI ui_text_cost;
    [ SerializeField ] TextMeshProUGUI ui_text_level;
    [ SerializeField ] UnityEvent ui_event_onLevelLoaded;

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
    [ Button() ]
    public void Configure()
    {
		ui_event_onLevelLoaded.Invoke();
	}

    public void Configure( bool available, Color color, float cost, int level )
    {
		ui_button.interactable = false;
		this.available         = available;
		ui_text_cost.text           = cost.ToString();
		ui_text_cost.color          = color;

	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
