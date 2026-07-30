using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.SaveSystem
{
	public class SaveService : ISaveService
	{
		public SettingsSave LoadSettings() => Load<SettingsSave>("settings.json");
		public void SaveSettings(SettingsSave data) => Save(data, "settings.json");

		public LevelSave LoadLevelSettings() => Load<LevelSave>("levelSettings.json");
		public void SaveLevelSettings(LevelSave data) => Save(data, "levelSettings.json");

		public CoinSave LoadCoinSettings() => Load<CoinSave>("coinSettings.json");
		public void SaveCoinSettings(CoinSave data) => Save(data, "coinSettings.json");

		public ShopSave LoadShopSettings() => Load<ShopSave>("shopSettings.json");
		public void SaveShopSettings(ShopSave data) => Save(data, "shopSettings.json");

		public void DeleteSaveFile(string fileName)
		{
			string path = GetFilePath(fileName);

			if (File.Exists(path))
			{
				File.Delete(path);

				Debug.Log($"Deleted save file: {path}");
			}
			else
			{
				Debug.LogWarning($"Save file not found: {path}");
			}
		}

		private static void Save<T>(T data, string fileName)
		{
			try
			{
				string path = GetFilePath(fileName);
				string jsonData = JsonUtility.ToJson(data, true);

				File.WriteAllText(path, jsonData);

				Debug.Log($"{typeof(T).Name} saved to {path}");
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to save {typeof(T).Name}: {e.Message}");
			}
		}

		private static T Load<T>(string fileName) where T : new()
		{
			string path = GetFilePath(fileName);

			if (File.Exists(path))
			{
				try
				{
					string jsonData = File.ReadAllText(path);
					return JsonUtility.FromJson<T>(jsonData);
				}
				catch (Exception e)
				{
					Debug.LogError($"Failed to load {typeof(T).Name}: {e.Message}");
					return new T();
				}
			}

			return new T();
		}

		private static string GetFilePath(string fileName)
		{
			return Path.Combine(Application.persistentDataPath, fileName);
		}
	}
}