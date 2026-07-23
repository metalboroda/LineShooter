using System.Threading;
using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.ScriptableObjects.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Canvases
{
	public class WinCanvasHandler : CanvasHandlerBase
	{
		[Header("Data")]
		[SerializeField] private LevelDataSo levelData;
		[SerializeField] private CoinDataSo coinData;

		[Header("Coin Counter")]
		[SerializeField] private Text coinCounterText;

		[Header("References")]
		[SerializeField] private Button mainMenuButton;
		[SerializeField] private Button nextLevelButton;
		[SerializeField] private Button restartButton;
		[Space]
		[SerializeField] private GameObject[] stars;

		protected override void OnShown()
		{
			nextLevelButton.onClick.AddListener(() => {
				EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
				{
					ButtonType = UiButtonType.NextLevel,
				});
			});

			mainMenuButton.onClick.AddListener(() => {
				EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
				{
					ButtonType = UiButtonType.MainMenu,
				});
			});

			restartButton.onClick.AddListener(() => {
				EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
				{
					ButtonType = UiButtonType.Restart
				});
			});

			DoSetRating(levelData.CurrentLevelRating, this.GetCancellationTokenOnDestroy()).Forget();
		}

		protected override void OnHidden()
		{
			nextLevelButton.onClick.RemoveAllListeners();
			mainMenuButton.onClick.RemoveAllListeners();
			restartButton.onClick.RemoveAllListeners();
		}

		private void Update()
		{
			if (!IsVisible) return;

			coinCounterText.text = coinData.CoinAmount.ToString();
		}

		private async UniTaskVoid DoSetRating(int rating, CancellationToken token)
		{
			bool wasCancelled = await UniTask.Yield(PlayerLoopTiming.Update, token).SuppressCancellationThrow();

			if (wasCancelled) return;

			if (stars.Length == 0) return;

			const int maxRating = 3;

			rating = Mathf.Clamp(rating, 0, maxRating);

			for (int i = 0; i < maxRating; i++)
			{
				stars[i].SetActive(i < rating);
			}
		}
	}
}
