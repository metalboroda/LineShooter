using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.ScriptableObjects.Infrastructure;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI.Canvases
{
	public class GameCanvasHandler : CanvasHandlerBase
	{
		[Inject] private CoinDataSo _coinData;

		[Header("Damage Vignette Settings")]
		[SerializeField] private float damageDuration;
		[Space]
		[SerializeField] private Color normalVignetteColor;
		[SerializeField] private Color damagedVignetteColor;
		[Space]
		[SerializeField] private Image damageVignette;
        
		[Header("Coin Counter")]
		[SerializeField] private Text coinCounterText;
		[Space]
		[SerializeField] private Image coinCounterIcon;

		[Header("References")]
		[SerializeField] private Button pauseButton;

		protected override void OnShown()
		{
			pauseButton.onClick.AddListener(() =>
			{
				EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
				{
					ButtonType = UiButtonType.Pause
				});
			});

			_coinData.CoinAmountChanged += OnCoinAmountChanged;

			coinCounterText.text = _coinData.CoinAmount.ToString();
		}

		protected override void OnHidden()
		{
			pauseButton.onClick.RemoveAllListeners();

			_coinData.CoinAmountChanged -= OnCoinAmountChanged;
		}

		private void OnCoinAmountChanged(int coinAmount)
		{
			coinCounterText.text = coinAmount.ToString();
		}
	}
}