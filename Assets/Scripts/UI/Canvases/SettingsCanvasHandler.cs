using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI.Canvases
{
    public class SettingsCanvasHandler : CanvasHandlerBase
    {
        [Inject] private ISaveService _saveService;

        [Header("References")]
        [SerializeField] private Button backButton;
        [Space(20)]
        [SerializeField] private Button musicButton;
        [SerializeField] private GameObject musicOn;
        [SerializeField] private GameObject musicOff;
        [Space(20)]
        [SerializeField] private Button sfxButton;
        [SerializeField] private GameObject sfxOn;
        [SerializeField] private GameObject sfxOff;

        private SettingsSave _settings;

        protected override void OnShown()
        {
            LoadSettings();

            backButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.Back
                });
            });

            musicButton.onClick.AddListener(ToggleMusic);
            sfxButton.onClick.AddListener(ToggleSfx);
        }

        protected override void OnHidden()
        {
            backButton.onClick.RemoveAllListeners();
            musicButton.onClick.RemoveAllListeners();
            sfxButton.onClick.RemoveAllListeners();
        }

        private void LoadSettings()
        {
            _settings = _saveService.LoadSettings();

            UpdateMusicButton();
            UpdateSfxButton();
        }

        private void ToggleMusic()
        {
            _settings.SaveMusic(!_settings.isMusicOn);

            EventBus<UIEvents.UiMusicClicked>.Raise(new UIEvents.UiMusicClicked());
            EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked());

            _saveService.SaveSettings(_settings);

            UpdateMusicButton();
        }

        private void ToggleSfx()
        {
            _settings.SaveSfx(!_settings.isSfxOn);

            EventBus<UIEvents.UiSfxClicked>.Raise(new UIEvents.UiSfxClicked());
            EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked());

            _saveService.SaveSettings(_settings);

            UpdateSfxButton();
        }

        private void UpdateMusicButton()
        {
            musicOn.SetActive(_settings.isMusicOn);
            musicOff.SetActive(!_settings.isMusicOn);
        }

        private void UpdateSfxButton()
        {
            sfxOn.SetActive(_settings.isSfxOn);
            sfxOff.SetActive(!_settings.isSfxOn);
        }
    }
}