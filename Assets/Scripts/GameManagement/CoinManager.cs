using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.ScriptableObjects.Infrastructure;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GameManagement
{
	public class CoinManager : MonoBehaviour, ICoinWallet
	{
		[Inject] private CoinDataSo _coinDataSo;
		[Inject] private ISaveService _saveService;
		
		[Header("Debug")]
		[SerializeField] private bool debug;
		[SerializeField] private int startCoinAmount = 999999;

		private int _coinAmount;
		private CoinSave _coinSave;
		
		private EventBinding<CurrencyEvents.CoinIncreased> _coinIncreased;
		private EventBinding<UIEvents.CanvasChanged> _canvasChanged;
		private EventBinding<UIEvents.UIButtonClicked> _uiButtonClicked;

		private void Awake()
		{
			_coinDataSo.CoinAmount = 0;

			if (debug)
				_coinAmount = startCoinAmount;
			else
				LoadCoins();
		}

		private void Start()
		{
			_coinDataSo.CoinAmount = _coinAmount;
		}

		private void OnEnable()
		{
			_coinIncreased = new EventBinding<CurrencyEvents.CoinIncreased>(OnCoinIncreased);
			EventBus<CurrencyEvents.CoinIncreased>.Register(_coinIncreased);
			_canvasChanged = new EventBinding<UIEvents.CanvasChanged>(OnCanvasChanged);
			EventBus<UIEvents.CanvasChanged>.Register(_canvasChanged);
			_uiButtonClicked = new EventBinding<UIEvents.UIButtonClicked>(OnUiButtonClicked);
			EventBus<UIEvents.UIButtonClicked>.Register(_uiButtonClicked);
		}

		private void OnDisable()
		{
			EventBus<CurrencyEvents.CoinIncreased>.Unregister(_coinIncreased);
			EventBus<UIEvents.CanvasChanged>.Unregister(_canvasChanged);
			EventBus<UIEvents.UIButtonClicked>.Unregister(_uiButtonClicked);
		}

		public int CoinAmount => _coinAmount;

		public bool CanAfford(int price) => price <= _coinAmount;

		public bool TryPurchase(int price)
		{
			if (price > _coinAmount) return false;

			_coinAmount -= price;

			if (_coinAmount < 0)
				_coinAmount = 0;

			_coinDataSo.CoinAmount = _coinAmount;

			SaveCoins();

			return true;
		}

		private void OnCoinIncreased(CurrencyEvents.CoinIncreased eventData)
		{
			_coinDataSo.CoinAmount += eventData.CoinAmount;
			_coinAmount = _coinDataSo.CoinAmount;
		}

		private void OnCanvasChanged(UIEvents.CanvasChanged eventData)
		{
			_coinDataSo.CoinAmount = _coinAmount;
		}

		private void OnUiButtonClicked(UIEvents.UIButtonClicked eventData)
		{
			if (eventData.ButtonType is UiButtonType.Restart or UiButtonType.MainMenu)
			{
				LoadCoins();

				_coinDataSo.CoinAmount = _coinAmount;
			}
		}

		private void SaveCoins()
		{
			_coinSave ??= new CoinSave();
			_coinSave.overallCoinAmount = _coinAmount;

			_saveService.SaveCoinSettings(_coinSave);
		}

		private void LoadCoins()
		{
			_coinSave = _saveService.LoadCoinSettings();
			_coinAmount = _coinSave.overallCoinAmount;
		}
	}
}