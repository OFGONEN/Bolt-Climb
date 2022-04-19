/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "incremental_system", menuName = "FF/Data/Incremental/System" ) ]
public class UpgradeSystem : ScriptableObject
{
#region Fields
    [ SerializeField ] Currency currency;
    [ SerializeField ] IncrementalDurability incremental_durability;
    [ SerializeField ] IncrementalVelocity incremental_velocity;
    [ SerializeField ] IncrementalCurrency incremental_currency;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void UnlockIncremental_Durability()
    {
		var currentIndex = PlayerPrefs.GetInt( ExtensionMethods.durability_index, 0 );
		currency.SharedValue -= incremental_durability.ReturnIncremental( currentIndex ).incremental_cost;

		PlayerPrefs.SetInt( ExtensionMethods.durability_index, Mathf.Min( currentIndex + 1, incremental_durability.IncrementalCount ) );
	}

    public void UnlockIncremental_Velocity()
    {
		var currentIndex = PlayerPrefs.GetInt( ExtensionMethods.velocity_index, 0 );
		currency.SharedValue -= incremental_velocity.ReturnIncremental( currentIndex ).incremental_cost;

		PlayerPrefs.SetInt( ExtensionMethods.velocity_index, Mathf.Min( currentIndex + 1, incremental_velocity.IncrementalCount ) );
	}

    public void UnlockIncremental_Currency()
    {
		var currentIndex = PlayerPrefs.GetInt( ExtensionMethods.currency_index, 0 );
		currency.SharedValue -= incremental_currency.ReturnIncremental( currentIndex ).incremental_cost;

		PlayerPrefs.SetInt( ExtensionMethods.currency_index, Mathf.Min( currentIndex + 1, incremental_currency.IncrementalCount ) );
	}

	public void SetUpIncrementalButton_Durability( IncrementalButton incrementalButton )
	{
		var index     = PlayerPrefs.GetInt( ExtensionMethods.durability_index, 0 );
		var available = CanShow_Durability() && CanAfford_Durability();
		var color     = available ? Color.green : Color.red;
		var cost      = incremental_durability.ReturnIncremental( index  ).incremental_cost;

		incrementalButton.Configure( available, color, cost, index );
	}

	public void SetUpIncrementalButton_Velocity( IncrementalButton incrementalButton )
	{
		var index     = PlayerPrefs.GetInt( ExtensionMethods.velocity_index, 0 );
		var available = CanShow_Velocity() && CanAfford_Velocity();
		var color     = available ? Color.green : Color.red;
		var cost      = incremental_velocity.ReturnIncremental( index ).incremental_cost;

		incrementalButton.Configure( available, color, cost, index );
	}

	public void SetUpIncrementalButton_Currency( IncrementalButton incrementalButton )
	{
		var index     = PlayerPrefs.GetInt( ExtensionMethods.currency_index, 0 );
		var available = CanShow_Currency() && CanAfford_Currency();
		var color     = available ? Color.green : Color.red;
		var cost      = incremental_currency.ReturnIncremental( index ).incremental_cost;

		incrementalButton.Configure( available, color, cost, index );
	}
#endregion

#region Implementation
    bool CanAfford_Durability()
    {
		return currency.SharedValue >= incremental_durability.ReturnIncremental( PlayerPrefs.GetInt( ExtensionMethods.durability_index, 0 ) ).incremental_cost;
	}

    bool CanAfford_Velocity()
    {
		return currency.SharedValue >= incremental_velocity.ReturnIncremental( PlayerPrefs.GetInt( ExtensionMethods.velocity_index, 0 ) ).incremental_cost;
    }

    bool CanAfford_Currency()
    {
		return currency.SharedValue >= incremental_currency.ReturnIncremental( PlayerPrefs.GetInt( ExtensionMethods.currency_index, 0 ) ).incremental_cost;
    }

    bool CanShow_Durability()
    {
		return PlayerPrefs.GetInt( ExtensionMethods.durability_index, 0 ) < incremental_durability.IncrementalCount - 1;
	}

    bool CanShow_Velocity()
    {
		return PlayerPrefs.GetInt( ExtensionMethods.velocity_index, 0 ) < incremental_velocity.IncrementalCount - 1;
    }

    bool CanShow_Currency()
    {
		return PlayerPrefs.GetInt( ExtensionMethods.currency_index, 0 ) < incremental_currency.IncrementalCount - 1;
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
