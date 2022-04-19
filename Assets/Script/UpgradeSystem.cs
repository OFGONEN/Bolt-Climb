/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "incremental_system", menuName = "FF/Data/Incremental/System" ) ]
public class UpgradeSystem : ScriptableObject
{
#region Fields
    [ SerializeField ] Currency currency;
    [ SerializeField ] IncrementalDurability incremental_durability;
    [ SerializeField ] IncrementalVelocity incremental_velocity;
    [ SerializeField ] IncrementalCurrency incremental_curreny;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void UnlockIncremental_Durability()
    {
		var currentIndex = PlayerPrefs.GetInt( "durability_index", 0 );
		currency.SharedValue -= incremental_durability.ReturnIncremental( currentIndex ).incremental_cost;

		PlayerPrefs.SetInt( "durability_index", Mathf.Min( currentIndex + 1, incremental_durability.IncrementalCount ) );
	}

    public void UnlockIncremental_Velocity()
    {
		var currentIndex = PlayerPrefs.GetInt( "velocity_index", 0 );
		currency.SharedValue -= incremental_velocity.ReturnIncremental( currentIndex ).incremental_cost;

		PlayerPrefs.SetInt( "velocity_index", Mathf.Min( currentIndex + 1, incremental_velocity.IncrementalCount ) );
	}

    public void UnlockIncremental_Currency()
    {
		var currentIndex = PlayerPrefs.GetInt( "currency_index", 0 );
		currency.SharedValue -= incremental_curreny.ReturnIncremental( currentIndex ).incremental_cost;

		PlayerPrefs.SetInt( "currency_index", Mathf.Min( currentIndex + 1, incremental_curreny.IncrementalCount ) );
	}

    public bool CanAfford_Durability()
    {
		return currency.SharedValue >= incremental_durability.ReturnIncremental( PlayerPrefs.GetInt( "durability_index", 0 ) ).incremental_cost;

	}

    public bool CanAfford_Velocity()
    {
		return currency.SharedValue >= incremental_velocity.ReturnIncremental( PlayerPrefs.GetInt( "velocity_index", 0 ) ).incremental_cost;
    }

    public bool CanAfford_Currency()
    {
		return currency.SharedValue >= incremental_curreny.ReturnIncremental( PlayerPrefs.GetInt( "currency_index", 0 ) ).incremental_cost;
    }

    public bool CanShow_Durability()
    {
		return PlayerPrefs.GetInt( "durability_index", 0 ) < incremental_durability.IncrementalCount - 1;

	}

    public bool CanShow_Velocity()
    {
		return PlayerPrefs.GetInt( "velocity_index", 0 ) < incremental_velocity.IncrementalCount - 1;
    }

    public bool CanShow_Currency()
    {
		return PlayerPrefs.GetInt( "currency_index", 0 ) < incremental_curreny.IncrementalCount - 1;
    }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
