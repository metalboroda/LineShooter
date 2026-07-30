using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.GameManagement;
using Assets.Scripts.UI.Canvases;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI
{
	public class ShopItemHandler : MonoBehaviour
	{
		[Inject] private ICoinWallet _coinWallet;
		
		[Header("References")]
		[SerializeField] private Text priceText;
		[Space]
		[SerializeField] private Button buyButton;
		[SerializeField] private Button selectButton;
		[SerializeField] private Button selectedButton;
		[Space]
		[SerializeField] private Image itemImage;
		[Space]
		[SerializeField] private GameObject priceObject;

		private string _itemName;
		private int _price;
		private bool _selected;
		private bool _unlocked;
		private ShopItemConfig _shopItemConfig;

		private ShopCanvasHandler _shopCanvasHandler;

		private void OnEnable()
		{
			buyButton.onClick.AddListener(BuyItem);
			selectButton.onClick.AddListener(SelectItem);

			UpdateBuyButtonState();
		}

		private void OnDisable()
		{
			buyButton.onClick.RemoveAllListeners();
			selectButton.onClick.RemoveAllListeners();
		}

		public ShopItemHandler SetName(string newName)
		{
			_itemName = newName;
			return this;
		}

		public ShopItemHandler SetPrice(int price)
		{
			_price = price;
			priceText.text = price.ToString();

			UpdateBuyButtonState();
			return this;
		}

		public void SetImage(Sprite sprite)
		{
			if (itemImage)
			{
				itemImage.sprite = sprite;
				itemImage.gameObject.SetActive(true);
			}
		}

		public void SetUnlocked(bool unlocked)
		{
			_unlocked = unlocked;

			priceText.gameObject.SetActive(!unlocked);
			buyButton.gameObject.SetActive(!unlocked);
			selectButton.gameObject.SetActive(unlocked);
			priceObject.SetActive(!unlocked);
			selectedButton.gameObject.SetActive(unlocked && _selected);
		}

		public void SetSelected(bool selected)
		{
			_selected = selected;

			selectedButton.gameObject.SetActive(_unlocked && selected);
		}

		public string GetName() => _itemName;

		public ShopItemHandler SetShopItem(ShopItemConfig shopItemConfig)
		{
			_shopItemConfig = shopItemConfig;
			return this;
		}

		public ShopItemHandler SetShopCanvasHandler(ShopCanvasHandler shopCanvasHandler)
		{
			_shopCanvasHandler = shopCanvasHandler;
			return this;
		}

		private void BuyItem()
		{
			if (!_coinWallet.TryPurchase(_price)) return;

			UnlockItem();

			EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked());
		}

		private void UnlockItem()
		{
			_unlocked = true;

			priceObject.gameObject.SetActive(!_unlocked);
			priceText.gameObject.SetActive(!_unlocked);
			buyButton.gameObject.SetActive(!_unlocked);
			selectButton.gameObject.SetActive(_unlocked);
			_shopCanvasHandler.PurchaseItem(_shopItemConfig);
		}

		private void SelectItem()
		{
			_shopCanvasHandler.SelectItem(this);

			EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked());
		}

		private void UpdateBuyButtonState()
		{
			if (buyButton is null || _coinWallet is null) return;

			buyButton.interactable = _coinWallet.CanAfford(_price);

			if (!_unlocked)
				selectedButton.gameObject.SetActive(false);
		}
	}
}