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
		var currentIndex = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.durability_index, 0 );
		PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.durability_index, Mathf.Min( currentIndex + 1, incremental_durability.IncrementalCount) );
		PlayerPrefsUtility.Instance.AddInt( ExtensionMethods.durability_index_visual, 1 );


		currency.SharedValue -= incremental_durability.ReturnIncremental( currentIndex + 1 ).incremental_cost;
		currency.SaveCurrency();
	}

    public void UnlockIncremental_Velocity()
    {
		var currentIndex = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.velocity_index, 0 );
		PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.velocity_index, Mathf.Min( currentIndex + 1, incremental_velocity.IncrementalCount ) );
		PlayerPrefsUtility.Instance.AddInt( ExtensionMethods.velocity_index_visual, 1 );

		currency.SharedValue -= incremental_velocity.ReturnIncremental( currentIndex + 1 ).incremental_cost;
		currency.SaveCurrency();
	}

    public void UnlockIncremental_Currency()
    {
		var currentIndex = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.currency_index, 0 );
		PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.currency_index, Mathf.Min( currentIndex + 1, incremental_currency.IncrementalCount ) );

		currency.SharedValue -= incremental_currency.ReturnIncremental( currentIndex + 1 ).incremental_cost;
		currency.SaveCurrency();
	}

	public void SetUpIncrementalButton_Durability( IncrementalButton incrementalButton )
	{
		var index     = Mathf.Min( PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.durability_index, 0 ), incremental_durability.IncrementalCount - 1 );
		var available = CanShow_Durability() && CanAfford_Durability();
		var color     = available ? Color.green : Color.red;
		var cost      = incremental_durability.ReturnIncremental( Mathf.Min( index + 1, incremental_durability.IncrementalCount - 1 )  ).incremental_cost;

		incrementalButton.Configure( available, color, cost, PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.durability_index_visual, 0 ) );
	}

	public void SetUpIncrementalButton_Velocity( IncrementalButton incrementalButton )
	{
		var index     = Mathf.Min( PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.velocity_index, 0 ), incremental_velocity.IncrementalCount - 1 );
		var available = CanShow_Velocity() && CanAfford_Velocity();
		var color     = available ? Color.green : Color.red;
		var cost      = incremental_velocity.ReturnIncremental( Mathf.Min( index + 1, incremental_velocity.IncrementalCount - 1 ) ).incremental_cost;

		incrementalButton.Configure( available, color, cost, PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.velocity_index_visual, 0 ) );
	}

	public void SetUpIncrementalButton_Currency( IncrementalButton incrementalButton )
	{
		var index     = Mathf.Min( PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.currency_index, 0 ), incremental_currency.IncrementalCount - 1 );
		var available = CanShow_Currency() && CanAfford_Currency();
		var color     = available ? Color.green : Color.red;
		var cost      = incremental_currency.ReturnIncremental( Mathf.Min( index + 1, incremental_currency.IncrementalCount - 1 ) ).incremental_cost;

		incrementalButton.Configure( available, color, cost, index );
	}
#endregion

#region Implementation
    bool CanAfford_Durability()
    {
		return currency.SharedValue >= incremental_durability.ReturnIncremental( PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.durability_index, 0 ) + 1 ).incremental_cost;
	}

    bool CanAfford_Velocity()
    {
		return currency.SharedValue >= incremental_velocity.ReturnIncremental( PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.velocity_index, 0 ) + 1 ).incremental_cost;
    }

    bool CanAfford_Currency()
    {
		return currency.SharedValue >= incremental_currency.ReturnIncremental( PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.currency_index, 0 ) + 1 ).incremental_cost;
    }

    bool CanShow_Durability()
    {
		var index = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.durability_index, 0 );
		return index + 1 < incremental_durability.IncrementalCount && index + 1 < CurrentLevelData.Instance.levelData.incremental_cap_durability ;
	}

    bool CanShow_Velocity()
    {
		var index = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.velocity_index, 0 );
		return index + 1 < incremental_velocity.IncrementalCount && index + 1 < CurrentLevelData.Instance.levelData.incremental_cap_velocity;
    }

    bool CanShow_Currency()
    {
		return PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.currency_index, 0 ) + 1 < incremental_currency.IncrementalCount;
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
