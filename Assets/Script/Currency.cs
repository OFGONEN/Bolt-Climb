/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ InlineEditor, CreateAssetMenu( fileName = "currency", menuName = "FF/Data/Game/Currency" ) ]
public class Currency : SharedFloatNotifier
{
#region Fields
    [ SerializeField ] UICurrencyPool pool_currency_ui;
    [ SerializeField ] IncrementalCurrency currency_incremental;
    [ ShowInInspector, ReadOnly ] IncrementalCurrencyData currency_data;
    float currency_cooldown = 0;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void SetCurrencyData()
    {
		currency_data     = currency_incremental.ReturnIncremental( PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.currency_index, 0 ) );
		currency_cooldown = 0;
	}

    public void OnIncrease()
    {
        if( Time.time > currency_cooldown )
        {
			SharedValue       += currency_data.incremental_currency_value;
			currency_cooldown  = Time.time + currency_data.incremental_currency_rate;

			pool_currency_ui.GetEntity().Spawn( $"+{currency_data.incremental_currency_value}"); // Spawn currency ui
			// pool_currency_ui.GetEntity().Spawn( $"+${currency_data.incremental_currency_value}"); // Spawn currency ui
		}
    }

    public void OnDecrease( float amount )
    {
		SharedValue = Mathf.Max( 0, sharedValue - amount );
	}

    public void SaveCurrency()
    {
		PlayerPrefsUtility.Instance.SetFloat( ExtensionMethods.currency, SharedValue );
    }

    public void LoadCurrency()
    {
		SharedValue =  PlayerPrefsUtility.Instance.GetFloat( ExtensionMethods.currency, 0 );
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}