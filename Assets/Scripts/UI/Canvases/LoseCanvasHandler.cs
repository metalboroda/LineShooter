using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.ScriptableObjects.Infrastructure;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI.Canvases
{
	public class LoseCanvasHandler : CanvasHandlerBase
	{
		[Inject] private LevelDataSo _levelData;
		[Inject] private CoinDataSo _coinData;

		[Header("Coin Counter")]
		[SerializeField] private Text coinCounterText;
        
		[Header("References")]
		[SerializeField] private Button mainMenuButton;
		[SerializeField] private Button restartButton;

		protected override void OnShown()
		{
			restartButton.onClick.AddListener(() =>
			{
				EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
				{
					ButtonType = UiButtonType.Restart
				});
			});

			mainMenuButton.onClick.AddListener(() =>
			{
				EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
				{
					ButtonType = UiButtonType.MainMenu
				});
			});
		}

		protected override void OnHidden()
		{
			restartButton.onClick.RemoveAllListeners();
			mainMenuButton.onClick.RemoveAllListeners();
		}

		private void Update()
		{
			if (!IsVisible) return;

			coinCounterText.text = _coinData.CoinAmount.ToString();
		}
	}
}