/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

public class Shatter : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] ShatterPool pool_shatter;

  [ Title( "Info" ) ]
    [ SerializeField, ReadOnly ] public Rigidbody[] shatter_rigidbodies;
    [ SerializeField, ReadOnly ] public Vector3[] shatter_positions;
    [ SerializeField, ReadOnly ] public Vector3[] shatter_rotations;
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
    [ Button() ]
    private void CacheRigidbodies()
    {
        shatter_rigidbodies = GetComponentsInChildren< Rigidbody >();

		shatter_positions = new Vector3[ shatter_rigidbodies.Length ];
		shatter_rotations = new Vector3[ shatter_rigidbodies.Length ];

		for( var i = 0; i < shatter_rigidbodies.Length; i++ )
        {
			shatter_positions[ i ] = shatter_rigidbodies[ i ].transform.localPosition;
			shatter_rotations[ i ] = shatter_rigidbodies[ i ].transform.localEulerAngles;
		}
    }
#endif
#endregion
}
