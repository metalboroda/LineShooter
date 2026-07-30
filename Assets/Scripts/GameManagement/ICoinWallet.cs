namespace Assets.Scripts.GameManagement
{
	public interface ICoinWallet
	{
		public int CoinAmount { get; }

		public bool CanAfford(int price);

		public bool TryPurchase(int price);
	}
}