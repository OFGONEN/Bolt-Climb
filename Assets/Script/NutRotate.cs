/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FFStudio;
using Sirenix.OdinInspector;

public class NutRotate : MonoBehaviour
{
#region Fields
    [ SerializeField ] UnityEvent onRotateComplete;
    [ SerializeField ] Transform rotateTransform;
    UnityMessage updateMethod;

	Vector3 startRotate;
    float speed;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		updateMethod = ExtensionMethods.EmptyMethod;

		startRotate = rotateTransform.localEulerAngles;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
    public void StartRotate()
    {
		updateMethod = Rotate;
	}
#endregion

#region Implementation
    void Rotate()
    {
		speed += GameSettings.Instance.nut_unlock_rotate_speed * Time.deltaTime;
		rotateTransform.Rotate( Vector3.up * speed * Time.deltaTime, Space.Self );

        if( speed >= GameSettings.Instance.nut_unlock_rotate_speed_target )
        {
			updateMethod = ExtensionMethods.EmptyMethod;
			rotateTransform.localEulerAngles = startRotate;
			onRotateComplete.Invoke();

            FFLogger.Log( "Rotate Complete" );
		}
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}