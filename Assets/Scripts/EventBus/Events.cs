namespace Assets.Scripts.EventBus
{
    public static class Events
    {
        #region Audio
        public struct VoiceoverPlayed : IEvent
        {
            public bool IsVoiceoverPlayed;
        }
        #endregion

        #region CoinManager
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
        #endregion

        #region Shop
        public struct ShopItemSelected : IEvent {}
        #endregion
    }
}