/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FFStudio;
using Sirenix.OdinInspector;

public class ShopSlot_Skin : MonoBehaviour
{
#region Fields
  [ Title( "Setup" ) ]
    [ SerializeField ] int shop_index; 
    [ SerializeField ] SharedIntNotifier notif_shop_page; 
    [ SerializeField ] SkinLibrary skin_library; 
    [ SerializeField ] Currency currency; 

  [ Title( "UI Elements" ) ]
    [ SerializeField ] Button slot_button; 
    [ SerializeField ] Image slot_background; 
    [ SerializeField ] Image slot_background_outline; 
    [ SerializeField ] Image slot_skin; 
    [ SerializeField ] RectTransform slot_skin_cost_background; 
    [ SerializeField ] TextMeshProUGUI slot_skin_cost;

	int slot_index;
	int slot_cost;
	bool slot_owned;
    bool slot_selected;
    bool slot_purchasable;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public void OnConfigure()
    {
		    slot_index = shop_index + notif_shop_page.SharedValue * ExtensionMethods.ui_shop_slot_count;
		var skin       = skin_library.GetSkin();

        if( slot_index >= skin.skin_data_store.Length )
			Disable();
        else
        {
		    var index_skin   = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.nut_skin_index, 0 );
		    slot_owned       = PlayerPrefsUtility.Instance.GetInt( ExtensionMethods.nut_skin_owned_index + slot_index, 0 ) == 1; // skin_0
		    slot_selected    = index_skin == slot_index + 1;
			slot_cost        = skin_library.GetSkinData( slot_index + 1 ).skin_cost;
			slot_purchasable = slot_cost >= currency.SharedValue;

			Configure( skin );
        }
	}

    public void OnSelect()
    {
        if( slot_owned )
			SelectSlot();
		else if( slot_purchasable )
        {
			currency.SharedValue -= slot_cost;
			SelectSlot();
		}
    }
#endregion

#region Implementation
    void SelectSlot()
    {
		PlayerPrefsUtility.Instance.SetInt( ExtensionMethods.nut_skin_index, slot_index + 1 );
		slot_background_outline.enabled = true;
    }

    void Disable()
    {
		slot_button.interactable        = false;
		slot_background.enabled         = false;
		slot_background_outline.enabled = false;
		slot_skin.enabled               = false;
		slot_skin_cost_background.gameObject.SetActive( false );
	}

    void Configure( Skin skin )
    {
		slot_background.enabled = true;
		slot_skin.enabled       = true;
		slot_skin_cost_background.gameObject.SetActive( true );

		slot_skin.sprite = skin.skin_data_store[ slot_index ].skin_texture;

        if( slot_owned )
        {
			slot_skin_cost.text = slot_selected ? "Selected" : "Owned";
			slot_skin_cost.color = slot_selected ? Color.yellow : Color.green;
			slot_button.interactable = !slot_selected;
		}
        else
        {
			slot_skin_cost.text      = slot_cost.ToString();
			slot_skin_cost.color     = slot_purchasable ? Color.white : Color.red;
			slot_button.interactable = slot_purchasable;
		}

		slot_background_outline.enabled = slot_selected;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}