using Assets.Scripts.EventBus;

namespace Assets.Scripts.EventsFolder
{
	public static class CurrencyEvents
	{
		public struct CoinIncreased : IEvent
		{
			public int CoinAmount;
		}

		public struct BuyRequest : IEvent
		{
			public string RequestName;
			public int Price;
		}

		public struct BuyResponse : IEvent
		{
			public string ResponseName;
			public bool Response;
		}
	}
}