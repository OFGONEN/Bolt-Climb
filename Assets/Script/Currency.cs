/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "currency", menuName = "FF/Data/Game/Currency" ) ]
public class Currency : SharedFloatNotifier
{
#region Fields
    [ SerializeField, ReadOnly ] IncrementalCurrencyData currency_data;
    [ ShowInInspector, ReadOnly ] float currency_cooldown = 0;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void SetCurrencyData( IncrementalCurrencyData data )
    {
		currency_data     = data;
		currency_cooldown = 0;
	}

    public void OnIncrease()
    {
        if( Time.time > currency_cooldown )
        {
			SharedValue       += currency_data.incremental_currency_value;
			currency_cooldown  = Time.time + currency_data.incremental_currency_rate;
		}
    }

    public void OnDecrease( float amount )
    {
		SharedValue = Mathf.Max( 0, sharedValue - amount );
	}

    public void SaveCurrency()
    {
		PlayerPrefs.SetFloat( "currency", SharedValue );
    }

    public void LoadCurrency()
    {
		SharedValue =  PlayerPrefs.GetFloat( "currency", 0 );
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}