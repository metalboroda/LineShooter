using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Canvases
{
    public class PauseCanvasHandler : CanvasHandlerBase
    {
        [Header("References")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button settingsButton;

        protected override void OnShown()
        {
            resumeButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.ResumeGame,
                });
            });

            restartButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.Restart,
                });
            });

            mainMenuButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.MainMenu,
                });
            });

            settingsButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.Settings,
                });
            });
        }

        protected override void OnHidden()
        {
            resumeButton.onClick.RemoveAllListeners();
            restartButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
        }
    }
}