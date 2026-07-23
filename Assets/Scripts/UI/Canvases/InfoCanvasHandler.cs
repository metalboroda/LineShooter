using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Canvases
{
    public class InfoCanvasHandler : CanvasHandlerBase
    {
        [Header("Data")]
        [SerializeField] private Button backButton;

        protected override void OnShown()
        {
            backButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.Back
                });
            });
        }

        protected override void OnHidden()
        {
            backButton.onClick.RemoveAllListeners();
        }
    }
}