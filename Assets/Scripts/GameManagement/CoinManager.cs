using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.ScriptableObjects.Infrastructure;
using UnityEngine;

namespace Assets.Scripts.GameManagement
{
	public class CoinManager : MonoBehaviour
	{
		[Header("Data")]
		[SerializeField] private CoinDataSo coinDataSo;

		[Header("Debug")]
		[SerializeField] private bool debug;
		[SerializeField] private int startCoinAmount = 999999;

		private int _coinAmount;

		private CoinSave _coinSave;

		private EventBinding<Events.BuyRequest> _buyRequest;
		private EventBinding<Events.CoinIncreased> _coinIncreased;
		private EventBinding<UIEvents.CanvasChanged> _canvasChanged;
		private EventBinding<UIEvents.UIButtonClicked> _uiButtonClicked;

		private void Awake()
		{
			coinDataSo.CoinAmount = 0;

			if (debug)
				_coinAmount = startCoinAmount;
			else
				LoadCoins();
		}

		private void Start()
		{
			coinDataSo.CoinAmount = _coinAmount;
		}

		private void OnEnable()
		{
			_buyRequest = new EventBinding<Events.BuyRequest>(OnBuyRequest);
			EventBus<Events.BuyRequest>.Register(_buyRequest);
			_coinIncreased = new EventBinding<Events.CoinIncreased>(OnCoinIncreased);
			EventBus<Events.CoinIncreased>.Register(_coinIncreased);
			_canvasChanged = new EventBinding<UIEvents.CanvasChanged>(OnCanvasChanged);
			EventBus<UIEvents.CanvasChanged>.Register(_canvasChanged);
			_uiButtonClicked = new EventBinding<UIEvents.UIButtonClicked>(OnUiButtonClicked);
			EventBus<UIEvents.UIButtonClicked>.Register(_uiButtonClicked);
		}

		private void OnDisable()
		{
			EventBus<Events.BuyRequest>.Unregister(_buyRequest);
			EventBus<Events.CoinIncreased>.Unregister(_coinIncreased);
			EventBus<UIEvents.CanvasChanged>.Unregister(_canvasChanged);
			EventBus<UIEvents.UIButtonClicked>.Unregister(_uiButtonClicked);
		}

		private void OnBuyRequest(Events.BuyRequest buyRequest)
		{
			if (buyRequest.Price > _coinAmount) return;

			EventBus<Events.BuyResponse>.Raise(new Events.BuyResponse
			{
				ResponseName = buyRequest.RequestName,
				Response = true
			});

			_coinAmount -= buyRequest.Price;

			if (_coinAmount < 0)
			{
				_coinAmount = 0;
			}

			coinDataSo.CoinAmount = _coinAmount;

			SaveCoins();
		}

		private void OnCoinIncreased(Events.CoinIncreased coinIncreased)
		{
			coinDataSo.CoinAmount += coinIncreased.CoinAmount;
			_coinAmount = coinDataSo.CoinAmount;
		}

		private void OnCanvasChanged(UIEvents.CanvasChanged eventData)
		{
			coinDataSo.CoinAmount = _coinAmount;
		}

		private void OnUiButtonClicked(UIEvents.UIButtonClicked eventData)
		{
			if (eventData.ButtonType is UiButtonType.Restart or UiButtonType.MainMenu)
			{
				LoadCoins();

				coinDataSo.CoinAmount = _coinAmount;
			}
		}

		private void SaveCoins()
		{
			_coinSave ??= new CoinSave();
			_coinSave.overallCoinAmount = _coinAmount;

			SaveManager.SaveCoinSettings(_coinSave);
		}

		private void LoadCoins()
		{
			_coinSave = SaveManager.LoadCoinSettings();
			_coinAmount = _coinSave.overallCoinAmount;
		}
	}
}