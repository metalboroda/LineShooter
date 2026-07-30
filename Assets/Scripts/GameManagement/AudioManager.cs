using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.Hashes;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Assets.Scripts.GameManagement
{
    public class AudioManager : MonoBehaviour
    {
        [Inject] private ISaveService _saveService;
        [Inject] private AudioMixer _mixer;

        private const float MaxVolume = 0f;
        private const float MinVolume = -80f;
        private SettingsSave _gameSettings;
        
        private EventBinding<UIEvents.UiMusicClicked> _uiMusicClicked;
        private EventBinding<UIEvents.UiSfxClicked> _uiSfxClicked;

        private void OnEnable()
        {
            _uiMusicClicked = new EventBinding<UIEvents.UiMusicClicked>(SwitchMusicVolume);
            EventBus<UIEvents.UiMusicClicked>.Register(_uiMusicClicked);
            _uiSfxClicked = new EventBinding<UIEvents.UiSfxClicked>(SwitchSfxVolume);
            EventBus<UIEvents.UiSfxClicked>.Register(_uiSfxClicked);
        }

        private void OnDisable()
        {
            EventBus<UIEvents.UiMusicClicked>.Unregister(_uiMusicClicked);
            EventBus<UIEvents.UiSfxClicked>.Unregister(_uiSfxClicked);
        }

        private void Start()
        {
            LoadSettings();
            ApplyVolumeSettings();
        }

        private void LoadSettings()
        {
            _gameSettings = _saveService.LoadSettings() ?? new SettingsSave();
        }

        private void ApplyVolumeSettings()
        {
            _mixer.SetFloat(SettingsHashes.MusicVolume, _gameSettings.isMusicOn ? MaxVolume : MinVolume);
            _mixer.SetFloat(SettingsHashes.SfxVolume, _gameSettings.isSfxOn ? MaxVolume : MinVolume);
        }

        private void SaveSettings()
        {
            _saveService.SaveSettings(_gameSettings);
        }

        private void SwitchMusicVolume()
        {
            _gameSettings.SaveMusic(!_gameSettings.isMusicOn);

            _mixer.SetFloat(SettingsHashes.MusicVolume, _gameSettings.isMusicOn ? MaxVolume : MinVolume);

            SaveSettings();
        }

        private void SwitchSfxVolume()
        {
            _gameSettings.SaveSfx(!_gameSettings.isSfxOn);

            _mixer.SetFloat(SettingsHashes.SfxVolume, _gameSettings.isSfxOn ? MaxVolume : MinVolume);

            SaveSettings();
        }
    }
}