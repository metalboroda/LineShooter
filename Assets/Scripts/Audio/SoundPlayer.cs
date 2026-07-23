using Assets.Scripts.Components;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.GameManagement.GameStates;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundPlayer : MonoBehaviour
	{
		[Header("Clips")]
		[SerializeField] private AudioClip uiClickClip;
		[SerializeField] private AudioClip winClip;
		[SerializeField] private AudioClip loseClip;

		private AudioPlayerComponent _audioPlayerComponent;

		private EventBinding<GameEvents.GameStateChanged> _gameStateChanged;
		private EventBinding<UIEvents.UIButtonClicked> _uiButtonClicked;

		private void Awake()
		{
			_audioPlayerComponent = new AudioPlayerComponent()
				.SetAudioSource(GetComponent<AudioSource>())
				.SetMonoBehaviour(this);
		}

		private void OnEnable()
		{
			_gameStateChanged = new EventBinding<GameEvents.GameStateChanged>(OnGameStateChanged);
			EventBus<GameEvents.GameStateChanged>.Register(_gameStateChanged);
			_uiButtonClicked = new EventBinding<UIEvents.UIButtonClicked>(OnUiButtonClicked);
			EventBus<UIEvents.UIButtonClicked>.Register(_uiButtonClicked);
		}

		private void OnDisable()
		{
			EventBus<GameEvents.GameStateChanged>.Unregister(_gameStateChanged);
			EventBus<UIEvents.UIButtonClicked>.Unregister(_uiButtonClicked);
		}

		private void OnGameStateChanged(GameEvents.GameStateChanged eventData)
		{
			switch (eventData.State)
			{
				case GameWinState:
					_audioPlayerComponent.PlayClip(winClip, false, true);
					break;
				case GameLoseState:
					_audioPlayerComponent.PlayClip(loseClip, false, true);
					break;
			}
		}

		private void OnUiButtonClicked()
		{
			_audioPlayerComponent.PlayClip(uiClickClip, false, true);
		}
	}
}