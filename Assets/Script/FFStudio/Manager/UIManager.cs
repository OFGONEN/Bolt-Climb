/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace FFStudio
{
    public class UIManager : MonoBehaviour
    {
#region Fields
        [ Header( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedResponse;
        public EventListenerDelegateResponse levelCompleteResponse;
        public EventListenerDelegateResponse levelFailResponse;
        public EventListenerDelegateResponse tapInputListener;

        [ Header( "UI Elements" ) ]
        public UI_Patrol_Scale level_loadingBar_Scale;
        public TextMeshProUGUI level_count_text;
        public TextMeshProUGUI level_information_text;
        public UI_Patrol_Scale level_information_text_Scale;
        public Image loadingScreenImage;
        public Image foreGroundImage;
        public Image level_progress_icon_start;
        public Image level_progress_icon_end;
        public RectTransform tutorialObjects;
		public IncrementalButton[] incrementalButtons;

		[ Header( "Fired Events" ) ]
        public GameEvent levelRevealedEvent;
        public GameEvent loadNewLevelEvent;
        public GameEvent resetLevelEvent;
        public GameEvent event_shop_close;
        public ElephantLevelEvent elephantLevelEvent;
#endregion

#region Unity API
        private void OnEnable()
        {
            levelLoadedResponse.OnEnable();
            levelFailResponse.OnEnable();
            levelCompleteResponse.OnEnable();
            tapInputListener.OnEnable();
        }

        private void OnDisable()
        {
            levelLoadedResponse.OnDisable();
            levelFailResponse.OnDisable();
            levelCompleteResponse.OnDisable();
            tapInputListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedResponse.response   = LevelLoadedResponse;
            levelFailResponse.response     = LevelFailResponse;
            levelCompleteResponse.response = LevelCompleteResponse;
            tapInputListener.response      = ExtensionMethods.EmptyMethod;

			level_information_text.text = "Tap to Start";
        }
#endregion

#region API
        public void OnShopOpen()
        {
			tapInputListener.response = event_shop_close.Raise;
			level_information_text.text = "Tap To Close Shop";
		}

        public void OnShopClose()
        {
			DOVirtual.DelayedCall( 0.25f, () => tapInputListener.response = StartLevel );
			level_information_text.text = "Tap to Start";
		}
#endregion

#region Implementation
        private void LevelLoadedResponse()
        {
			IncrementalButtons_SetUp();
			float fade = IncrementalButtons_Available() ? 0 : 0.5f;
			var sequence = DOTween.Sequence()
								.Append( level_loadingBar_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
								.Append( loadingScreenImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
								.Join( foreGroundImage.DOFade( fade, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) );
                                IncrementalButtons_GoUp( sequence );
								sequence.AppendCallback( () => tapInputListener.response = StartLevel );

			level_count_text.text        = "Level " + CurrentLevelData.Instance.currentLevel_Shown;
			levelLoadedResponse.response = NewLevelLoaded;

			level_progress_icon_start.sprite = GameSettings.Instance.LevelProgressIconStart;
			level_progress_icon_end.sprite   = GameSettings.Instance.LevelProgressIconEnd;
		}

        private void NewLevelLoaded()
        {
			level_count_text.text       = "Level " + CurrentLevelData.Instance.currentLevel_Shown;
			level_information_text.text = "Tap to Start";

			var sequence = DOTween.Sequence();

			IncrementalButtons_SetUp();
			float fade = IncrementalButtons_Available() ? 0 : 0.5f;
			sequence.Append( foreGroundImage.DOFade( fade, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );
                    IncrementalButtons_GoUp( sequence );
					sequence.AppendCallback( () => tapInputListener.response = StartLevel );

            // elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            // elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
            // elephantLevelEvent.Raise();
        }

        private void LevelCompleteResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;

			level_information_text.text = "Completed \n\n Tap to Continue";

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = LoadNewLevel );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelCompleted;
            elephantLevelEvent.Raise();
        }

        private void LevelFailResponse()
        {
            var sequence = DOTween.Sequence();

			// Tween tween = null;
			level_information_text.text = "Level Failed \n\n Tap to Continue";

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
                    // .Append( tween ) // TODO: UIElements tween.
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
					.AppendCallback( () => tapInputListener.response = Resetlevel );

            elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
            elephantLevelEvent.elephantEventType = ElephantEvent.LevelFailed;
            elephantLevelEvent.Raise();
        }

		private void StartLevel()
		{
			IncrementalButtons_GoDown();

			foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration );

			level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration );
			level_information_text_Scale.Subscribe_OnComplete( levelRevealedEvent.Raise );

			tutorialObjects.gameObject.SetActive( false );

			tapInputListener.response = ExtensionMethods.EmptyMethod;

			elephantLevelEvent.level             = CurrentLevelData.Instance.currentLevel_Shown;
			elephantLevelEvent.elephantEventType = ElephantEvent.LevelStarted;
			elephantLevelEvent.Raise();
		}

		private void LoadNewLevel()
		{
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
			        .AppendCallback( loadNewLevelEvent.Raise );
		}

		private void Resetlevel()
		{
			tapInputListener.response = ExtensionMethods.EmptyMethod;

			var sequence = DOTween.Sequence();

			sequence.Append( foreGroundImage.DOFade( 1f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
			        .Join( level_information_text_Scale.DoScale_Target( Vector3.zero, GameSettings.Instance.ui_Entity_Scale_TweenDuration ) )
			        .AppendCallback( resetLevelEvent.Raise );
		}

        private void IncrementalButtons_SetUp()
        {
            for( var i = 0; i < incrementalButtons.Length; i++ )
            {
				incrementalButtons[ i ].Configure();
			}
        }

        private void IncrementalButtons_GoUp( Sequence sequence )
        {
			for( var i = 0; i < incrementalButtons.Length; i++ )
            {
				sequence.Join( incrementalButtons[ i ].GoToTargetPosition() );
			}
        }

        private void IncrementalButtons_GoDown()
        {
			for( var i = 0; i < incrementalButtons.Length; i++ )
            {
				incrementalButtons[ i ].GoToStartPosition();
			}
        }

        private bool IncrementalButtons_Available()
        {
			bool available = false;

            for( var i = 0; i < incrementalButtons.Length; i++ )
            {
				available = available || incrementalButtons[ i ].Availability;
			}

			return available;
		}
#endregion
    }
}