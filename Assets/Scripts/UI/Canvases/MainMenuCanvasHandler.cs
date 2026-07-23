using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Canvases
{
    public class MainMenuCanvasHandler : CanvasHandlerBase
    {
        [Header("References")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button infoButton;

        protected override void OnShown()
        {
            startButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.LevelSelector
                });
            });

            settingsButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.Settings
                });
            });

            shopButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.Shop
                });
            });

            infoButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.Info
                });
            });
        }

        protected override void OnHidden()
        {
            startButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
            shopButton.onClick.RemoveAllListeners();
            infoButton.onClick.RemoveAllListeners();
        }
    }
}