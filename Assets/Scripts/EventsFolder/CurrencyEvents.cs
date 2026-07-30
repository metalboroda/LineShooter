using Assets.Scripts.EventBus;

namespace Assets.Scripts.EventsFolder
{
	public static class CurrencyEvents
	{
		public struct CoinIncreased : IEvent
		{
			public int CoinAmount;
		}
	}
}