/* Created by and for usage of FF Studios (2021). */

using System.Text;
using UnityEngine;
using FFStudio;
using Shapes;
using TMPro;
using Sirenix.OdinInspector;

public class FollowLine : MonoBehaviour
{
#region Fields
    [ SerializeField ] LineType lineType;
    [ SerializeField ] SharedFloatNotifier notif_target_height;
    [ SerializeField ] RectTransform target_ui;
    [ SerializeField ] TextMeshProUGUI target_text;
    [ SerializeField ] Line target_line;
    [ SerializeField ] float target_offset;

 // Private
	StringBuilder stringBuilder = new StringBuilder( 8 );
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
		if( lineType == LineType.Vertical )
			UpdateLineVertical();
		else
			UpdateLineHorizontal();
	}
#endregion

#region API
    [ Button() ]
    public void UpdateLineVertical()
    {
		var height = notif_target_height.SharedValue;

		stringBuilder.Clear();
		stringBuilder.Append( notif_target_height.SharedValue.ToString( "F2" ) );
		stringBuilder.Append( 'm' );

		target_ui.transform.localPosition = Vector3.right * ( height + target_offset );
		target_text.text                  = stringBuilder.ToString();
		target_line.End                   = Vector3.right * height;
	}

    [ Button() ]
    public void UpdateLineHorizontal()
    {
		var height = notif_target_height.SharedValue;

		stringBuilder.Clear();
		stringBuilder.Append( notif_target_height.SharedValue.ToString( "F2" ) );
		stringBuilder.Append( 'm' );

		target_ui.transform.localPosition = Vector3.right * ( height + target_offset );
		target_text.text                  = stringBuilder.ToString();
		target_line.Start = new Vector3( height, transform.position.x, 0 );
		target_line.End   = new Vector3( height, 0, 0 );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}

public enum LineType
{
	Vertical,
	Horizontal
}