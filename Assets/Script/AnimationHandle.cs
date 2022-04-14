/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;

public class AnimationHandle : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] AnimationData[] animationDatas;

// Private
    RecycledSequence recycledSequence = new RecycledSequence();
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnValidate()
    {
        if( animationDatas != null && animationDatas.Length > 0 )
		    animationDatas = animationDatas.OrderBy( ( animData ) => animData.data_percentage ).ToArray();
	}
#endif
#endregion
}
