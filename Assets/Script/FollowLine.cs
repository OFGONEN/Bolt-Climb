/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Shapes;
using TMPro;
using Sirenix.OdinInspector;

public class FollowLine : MonoBehaviour
{
#region Fields
    [ SerializeField ] SharedFloatNotifier notif_target_height;
    [ SerializeField ] RectTransform target_ui;
    [ SerializeField ] TextMeshProUGUI target_text;
    [ SerializeField ] Line target_line;
    [ SerializeField ] float target_offset;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
		UpdateLine();
	}
#endregion

#region API
    [ Button() ]
    public void UpdateLine()
    {
		var height = notif_target_height.SharedValue;

		target_ui.transform.localPosition = Vector3.right * ( height + target_offset );
		target_text.text                  = notif_target_height.SharedValue.ToString( "F2");
		target_line.End                   = Vector3.right * height;
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}