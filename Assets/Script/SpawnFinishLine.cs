/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class SpawnFinishLine : MonoBehaviour
{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		var finishLine = GameObject.Instantiate( GameSettings.Instance.FinishLine );
		finishLine.transform.SetParent( transform );
        finishLine.transform.localPosition = Vector3.zero;
		finishLine.transform.localRotation = Quaternion.identity;
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
