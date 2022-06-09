/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class BoltDetach : MonoBehaviour
{
#region Fields
    [ SerializeField, ReadOnly ] Bolt bolt_connected;
#endregion

#region Properties
#endregion

#region Unity API
    public void DetachBolt()
    {
		bolt_connected?.Detach();
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    public void ConnectBolt( Bolt bolt )
    {
		bolt_connected = bolt;
	}
#endif
#endregion
}
