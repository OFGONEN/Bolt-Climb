/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FFStudio;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UISkillButton : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] SkillType[] skillTypes;
    [ BoxGroup( "Setup" ), SerializeField ] SkillData[] skillData;

    [ BoxGroup( "UI Elements" ), SerializeField ] Image skill_image;
    [ BoxGroup( "UI Elements" ), SerializeField ] TextMeshProUGUI skill_level;
    [ BoxGroup( "UI Elements" ), SerializeField ] TextMeshProUGUI skill_cost;
    [ BoxGroup( "UI Elements" ), SerializeField ] TextMeshProUGUI skill_description;

    RecycledTween recycledTween = new RecycledTween();
    int skillIndex = 0;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
		ToggleGraphics( false );
	}
#endregion

#region API
    public void EnableGraphics()
    {
		var levelSkillType = CurrentLevelData.Instance.levelData.skillType;

		skillIndex = FindSkillIndex( levelSkillType );

		if( skillIndex == -1 ) return;

		SetGraphics( skillIndex );
		ToggleGraphics( true );
		TweenInGraphic();
	}

    // Editor Call
    public void UnlockSkill()
    {
		skillData[ skillIndex ].Unlock();
	}

    // Editor Call
    public void OnSkillButton()
    {
		ToggleGraphics( false );
	}
#endregion

#region Implementation
    void ToggleGraphics( bool value )
    {
		skill_image.enabled       = value;
		skill_level.enabled       = value;
		skill_cost.enabled        = value;
		skill_description.enabled = value;
	}

    void SetGraphics( int index )
    {
		var data = skillData[ index ];

		skill_image.sprite     = data.Texture;
		skill_description.text = data.Description;
		skill_level.text       = data.NextLevel;
		skill_cost.text        = data.NextCost;
	}

    void TweenInGraphic()
    {
		var rectTransform = skill_image.rectTransform;

		rectTransform.localScale = Vector3.one * 0.8f;
		recycledTween.Recycle( rectTransform.DOPunchScale( Vector3.one, 0.35f ) );
	}

    int FindSkillIndex( SkillType type )
    {
		int index = -1;

        for( var i = 0; i < skillTypes.Length; i++ )
        {
            if( type == skillTypes[ i ] )
            {
				index = i;
				break;
			}
		}

		return index;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
