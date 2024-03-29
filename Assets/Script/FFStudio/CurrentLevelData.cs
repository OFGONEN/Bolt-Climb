﻿/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

namespace FFStudio
{
    public class CurrentLevelData : ScriptableObject
    {
#region Fields
		public int currentLevel_Real;
		public int currentLevel_Shown;
		public LevelData levelData;

        private static CurrentLevelData instance;

        private delegate CurrentLevelData ReturnCurrentLevel();
        private static ReturnCurrentLevel returnInstance = LoadInstance;

        public static CurrentLevelData Instance => returnInstance();

        public float BaseProgression => levelData.levelProgress_base;
        public float TargetProgression => levelData.levelProgress_base + levelData.levelProgress_current;
#endregion

#region API
		public void LoadCurrentLevelData()
		{
			if( currentLevel_Real > GameSettings.Instance.maxLevelCount )
				currentLevel_Real = Random.Range( 1, GameSettings.Instance.maxLevelCount );

			levelData = Resources.Load< LevelData >( "level_data_" + currentLevel_Real );

			levelData.incremental_set = levelData.incremental_set && PlayerPrefsUtility.Instance.GetInt( levelData.name, 0 ) == 0;
		}
#endregion

#region Implementation
        static CurrentLevelData LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< CurrentLevelData >( "level_current" );

			returnInstance = ReturnInstance;

            return instance;
        }

        static CurrentLevelData ReturnInstance()
        {
            return instance;
        }
#endregion
    }
}