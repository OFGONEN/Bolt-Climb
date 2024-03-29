/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
        [ Title( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedListener;
        public EventListenerDelegateResponse levelRevealedListener;
        public EventListenerDelegateResponse levelStartedListener;

        [ Title( "Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;
        public GameEvent event_level_started;

        [ Title( "Level Releated" ) ]
        public SharedFloatNotifier levelProgress;
        public SharedFloatNotifier notif_nut_point_fallDown;
        public SharedBoolNotifier notif_nut_is_onBolt;

#endregion

#region UnityAPI
        private void OnEnable()
        {
            levelLoadedListener.OnEnable();
            levelRevealedListener.OnEnable();
            levelStartedListener.OnEnable();
        }

        private void OnDisable()
        {
            levelLoadedListener.OnDisable();
            levelRevealedListener.OnDisable();
            levelStartedListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedListener.response   = LevelLoadedResponse;
            levelRevealedListener.response = LevelRevealedResponse;
            levelStartedListener.response  = LevelStartedResponse;
        }
#endregion

#region Implementation
        private void LevelLoadedResponse()
        {
            // Reset level related variables
			levelProgress.SetValue_NotifyAlways( 0 );
			notif_nut_is_onBolt.SetValue_DontNotify( true );
			notif_nut_point_fallDown.SetValue_DontNotify( 0 );

			var levelData = CurrentLevelData.Instance.levelData;

            // Set Active Scene.
			if( levelData.scene_overrideAsActiveScene )
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 1 ) );
            else
				SceneManager.SetActiveScene( SceneManager.GetSceneAt( 0 ) );
		}

        private void LevelRevealedResponse()
        {
			event_level_started.Raise();
		}

        private void LevelStartedResponse()
        {

        }
#endregion
    }
}