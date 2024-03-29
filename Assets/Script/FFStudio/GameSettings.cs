﻿/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
#region Fields (Settings)
    // Info: Use Title() attribute ONCE for every game-specific group of settings.

    // Info: Game related settings
        [ BoxGroup( "Game" ) ] public int game_level_count = 20;
        [ BoxGroup( "Game" ) ] public float game_tutorial_timeScale = 0.25f;
        [ BoxGroup( "Game" ) ] public Vector2 postProcess_vignette_intencity;
        [ BoxGroup( "Game" ) ] public Vector2 shader_range_crack = new Vector2( 0, 1 );
        [ BoxGroup( "Game" ) ] public GameObject[] game_finishLine;
        [ BoxGroup( "Game" ) ] public Sprite[] game_icon_progressionBar;
        [ BoxGroup( "Game" ) ] public Sprite[] game_icon_nut_background;
        [ BoxGroup( "Game" ) ] public Sprite[] game_icon_nut_foreground;

        [ BoxGroup( "Bolt" ) ] public int bolt_batch = 3;
        [ BoxGroup( "Bolt" ) ] public float bolt_height = 0.5f;
        [ BoxGroup( "Bolt" ) ] public float bolt_detach_waitTime;
        [ BoxGroup( "Bolt" ) ] public Vector2 bolt_detach_force;

        [ BoxGroup( "Movement" ) ] public float movement_rotation_cofactor   = 1f;
        [ BoxGroup( "Movement" ) ] public float movement_launchSpeed_minumum = 1f;
        [ BoxGroup( "Movement" ) ] public float movement_pathSpeed_minumum   = 1f;

        [ BoxGroup( "Nut" ), MinMaxSlider( 0, 50 ) ] public Vector2 nut_shatter_force_up;
        [ BoxGroup( "Nut" ), MinMaxSlider( 0, 50 ) ] public Vector2 nut_shatter_force;
        [ BoxGroup( "Nut" ), MinMaxSlider( 0, 50 ) ] public Vector2 nut_shatter_torque;
        [ BoxGroup( "Nut" ) ] public float nut_shatter_waitDuration = 2f;
        [ BoxGroup( "Nut" ) ] public Vector2 nut_levelEnd_force;
        [ BoxGroup( "Nut" ) ] public float nut_levelEnd_waitDuration = 2f;
        [ BoxGroup( "Nut" ) ] public float nut_unlock_rotate_speed = 1f;
        [ BoxGroup( "Nut" ) ] public float nut_unlock_rotate_speed_target = 20f;

    // Info: 3 groups below (coming from template project) are foldout by design: They should remain hidden.
		[ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_GameSettings;
        [ FoldoutGroup( "Remote Config" ) ] public bool useRemoteConfig_Components;

        public int maxLevelCount;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the filling for ui element"            ) ] public float ui_Entity_Filling_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Joy Stick"                                        ) ] public float ui_Entity_JoyStick_Gap;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text relative float height"                ) ] public float ui_PopUp_height;
		[ FoldoutGroup( "UI Settings" ), Tooltip( "Pop Up Text float duration"                       ) ] public float ui_PopUp_duration;
        [ FoldoutGroup( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;

        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_height;
        [ FoldoutGroup( "Debug" ) ] public float debug_ui_text_float_duration;


        public GameObject FinishLine => game_finishLine[ Mathf.Clamp( CurrentLevelData.Instance.levelData.finishLineIndex, 0, game_finishLine.Length - 1 ) ];

        public Sprite LevelNutIconBackground => game_icon_nut_background[ Mathf.Clamp( 
            CurrentLevelData.Instance.levelData.levelProgress_nutIcon_index, 
            0, 
            game_icon_nut_background.Length - 1 ) ];

        public Sprite LevelNutIconForeGround => game_icon_nut_foreground[ Mathf.Clamp( 
            CurrentLevelData.Instance.levelData.levelProgress_nutIcon_index, 
            0, 
            game_icon_nut_foreground.Length - 1 ) ];

         public Sprite LevelProgressIconStart => game_icon_progressionBar[ Mathf.Clamp( 
            CurrentLevelData.Instance.levelData.levelProgress_levelIcon_start_index, 
            0, 
            game_icon_progressionBar.Length - 1 ) ];

         public Sprite LevelProgressIconEnd => game_icon_progressionBar[ Mathf.Clamp( 
            CurrentLevelData.Instance.levelData.levelProgress_levelIcon_end_index, 
            0, 
            game_icon_progressionBar.Length - 1 ) ];
#endregion

#region Fields (Singleton Related)
        private static GameSettings instance;

        private delegate GameSettings ReturnGameSettings();
        private static ReturnGameSettings returnInstance = LoadInstance;

		public static GameSettings Instance => returnInstance();
#endregion

#region Implementation
        private static GameSettings LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< GameSettings >( "game_settings" );

			returnInstance = ReturnInstance;

			return instance;
		}

		private static GameSettings ReturnInstance()
        {
            return instance;
        }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
    }
}
