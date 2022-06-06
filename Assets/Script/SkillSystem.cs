/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Sirenix.OdinInspector;

[ CreateAssetMenu( fileName = "skill_system", menuName = "FF/Skill System" ) ]
public class SkillSystem : ScriptableObject
{
#region Fields
  [ Title( "Currency" ) ]
    [ SerializeField ] SkillData skill_currency_on_newBolt;
    [ SerializeField ] SkillData skill_currency_on_maxSpeed;
    [ SerializeField ] SkillData skill_currency_on_air;

  [ Title( "Durability" ) ]
    [ SerializeField ] SkillData skill_durability_on_newBolt;
    [ SerializeField ] SkillData skill_durability_on_maxSpeed;
    [ SerializeField ] SkillData skill_durability_on_path;

  [ Title( "Speed" ) ]
    [ SerializeField ] SkillData skill_speed_on_newBolt;
    [ SerializeField ] SkillData skill_speed_gravity;
    [ SerializeField ] SkillData skill_speed_path;

  [ Title( "Last Chance" ) ]
    [ SerializeField ] SkillData skill_lastChance_doubleJump;
    [ SerializeField ] SkillData skill_lastChance_Shatter;


  [ Title( "Shared Variables" ) ]
    [ SerializeField ] Currency property_currency;
    [ SerializeField ] Durability property_durability;
    [ SerializeField ] Velocity property_velocity;
#endregion

#region Properties
#endregion

#region Unity API
    public void OnAwake()
    {

    }
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
