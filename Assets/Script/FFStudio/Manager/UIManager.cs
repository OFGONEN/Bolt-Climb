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
		public Image foreGroundImage_Skill;
		public Image level_progress_icon_start;
        public Image level_progress_icon_end;
        public Image level_progress_nut_icon_background;
        public Image level_progress_nut_icon_foreground_base;
        public Image level_progress_nut_icon_foreground_fill;
        public TextMeshProUGUI level_progress_nut_progress;
        public RectTransform tutorialObjects;
        public RectTransform target_information_text;
		public IncrementalButton[] incrementalButtons;

	[ Header( "Nut Unlocked" ) ]
	 	public SkinLibrary skinLibrary;
		public TextMeshProUGUI nut_unlock_text;
		public TextMeshProUGUI nut_unlock_input_text;
		public Image nut_unlock_header;

	[ Header( "Fired Events" ) ]
        public GameEvent levelRevealedEvent;
        public GameEvent loadNewLevelEvent;
        public GameEvent resetLevelEvent;
        public GameEvent event_shop_close;
        public GameEvent event_nut_unlocked_start;
        public GameEvent event_nut_unlocked_end;
        public ElephantLevelEvent elephantLevelEvent;

		int level_progress_nut;
		Vector3 level_information_text_start;
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

			level_progress_nut_icon_background.enabled      = false;
			level_progress_nut_icon_foreground_base.enabled = false;
			level_progress_nut_icon_foreground_fill.enabled = false;
			level_progress_nut_progress.enabled             = false;

			nut_unlock_header.enabled     = false;
			nut_unlock_text.enabled       = false;
			nut_unlock_input_text.enabled = false;
		}
#endregion

#region API
        public void OnShopOpen()
        {
			level_information_text_start                  = level_information_text.rectTransform.position;
			level_information_text.rectTransform.position = target_information_text.position;

			tapInputListener.response = event_shop_close.Raise;
			level_information_text.text = "Tap To Close Shop";
			foreGroundImage.color = foreGroundImage.color.SetAlpha( 0 );
		}

        public void OnShopClose()
        {
			level_information_text.rectTransform.position = level_information_text_start;
			DOVirtual.DelayedCall( 0.25f, () => tapInputListener.response = StartLevel );
			level_information_text.text = "Tap to Start";
		}

		public void OnNutUnlockRotateStop()
		{
			nut_unlock_header.enabled     = true;
			nut_unlock_text.enabled       = true;
			nut_unlock_input_text.enabled = true;

			nut_unlock_text.text = $"{skinLibrary.GetGeometryName()} Nut Unlocked";

			nut_unlock_header.transform.DOPunchScale( Vector3.one, 0.35f )
				.OnComplete( () => tapInputListener.response = OnNutUnlockComplete );
		}

		public void OnNutEndLevel()
		{
			foreGroundImage_Skill.enabled = true;
			foreGroundImage_Skill.color = Color.white.SetAlpha( 0.5f );
			foreGroundImage_Skill.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration );
		}
#endregion

#region Implementation
		void OnNutUnlockComplete()
		{
			nut_unlock_header.enabled     = false;
			nut_unlock_text.enabled       = false;
			nut_unlock_input_text.enabled = false;

			event_nut_unlocked_end.Raise();
		}

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
			IncrementalButtons_SetUp();

			if( CurrentLevelData.Instance.levelData.incremental_set )
			{
				var sequence = DOTween.Sequence();
				sequence.Append( foreGroundImage.DOFade( 0, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
						.AppendCallback( event_nut_unlocked_start.Raise );
			}
			else
				NewLevelLoaded_Sequence();
			
			level_progress_icon_start.sprite = GameSettings.Instance.LevelProgressIconStart;
			level_progress_icon_end.sprite   = GameSettings.Instance.LevelProgressIconEnd;
		}

		public void NewLevelLoaded_Sequence()
		{
			level_information_text.text = "Tap to Start";

			float fade = IncrementalButtons_Available() ? 0 : 0.5f;
			var sequence = DOTween.Sequence();
			sequence.Append( foreGroundImage.DOFade( fade, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					.Append( level_information_text_Scale.DoScale_Start( GameSettings.Instance.ui_Entity_Scale_TweenDuration ) );
                    IncrementalButtons_GoUp( sequence );
					sequence.AppendCallback( () => tapInputListener.response = StartLevel );
		}

        private void LevelCompleteResponse()
        {
			foreGroundImage_Skill.enabled = false;
			var sequence = DOTween.Sequence();
			level_information_text.text = "Completed \n\n Tap to Continue";

			var targetProgression = CurrentLevelData.Instance.TargetProgression;

			sequence.Append( foreGroundImage.DOFade( 0.5f, GameSettings.Instance.ui_Entity_Fade_TweenDuration ) )
					// .Append( tween ) // TODO: UIElements tween.
					.AppendCallback( EnableNutProgressIcon )
                    .Append( level_progress_nut_icon_foreground_fill.DOFillAmount( targetProgression, GameSettings.Instance.ui_Entity_Filling_TweenDuration ).SetEase( Ease.Linear ) )
					.Join( DOTween.To( GetNutProgression, SetNutProgression, (int)( targetProgression * 100 ), GameSettings.Instance.ui_Entity_Filling_TweenDuration ).SetEase( Ease.Linear ))
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

			level_progress_nut_icon_background.enabled      = false;
			level_progress_nut_icon_foreground_base.enabled = false;
			level_progress_nut_icon_foreground_fill.enabled = false;
			level_progress_nut_progress.enabled             = false;

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

        public void IncrementalButtons_SetUp()
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

        private void EnableNutProgressIcon()
        {
			level_progress_nut_icon_background.sprite = GameSettings.Instance.LevelNutIconBackground;
			level_progress_nut_icon_foreground_base.sprite = GameSettings.Instance.LevelNutIconForeGround;
			level_progress_nut_icon_foreground_fill.sprite = GameSettings.Instance.LevelNutIconForeGround;

			level_progress_nut_icon_background.enabled      = true;
			level_progress_nut_icon_foreground_base.enabled = true;
			level_progress_nut_icon_foreground_fill.enabled = true;
			level_progress_nut_progress.enabled             = true;

			level_progress_nut = ( int )( CurrentLevelData.Instance.BaseProgression * 100 );
			level_progress_nut_progress.text = $"%{level_progress_nut}";
			level_progress_nut_icon_foreground_fill.fillAmount = CurrentLevelData.Instance.BaseProgression;
		}

		private int GetNutProgression()
		{
			return level_progress_nut;
		}

		private void SetNutProgression( int value )
		{
			level_progress_nut = value;
			level_progress_nut_progress.text = $"%{level_progress_nut}";
		}
#endregion
    }
}