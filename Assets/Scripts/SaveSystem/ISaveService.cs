namespace Assets.Scripts.SaveSystem
{
	public interface ISaveService
	{
		SettingsSave LoadSettings();
		public void SaveSettings(SettingsSave data);

		LevelSave LoadLevelSettings();
		public void SaveLevelSettings(LevelSave data);

		CoinSave LoadCoinSettings();
		public void SaveCoinSettings(CoinSave data);

		ShopSave LoadShopSettings();
		public void SaveShopSettings(ShopSave data);

		public void DeleteSaveFile(string fileName);
	}
}