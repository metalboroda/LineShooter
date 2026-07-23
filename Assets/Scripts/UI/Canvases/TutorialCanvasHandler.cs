using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Canvases
{
	public class TutorialCanvasHandler : CanvasHandlerBase
	{
		[Header("Objects")]
		[SerializeField] private GameObject[] tutorialObjects;

		[Header("References")]
		[SerializeField] private Button skipTutorialButton;
		[Space]
		[SerializeField] private CanvasHandlerBase gameCanvas;

		private int _currentTutorialIndex;

		private void Awake()
		{
			foreach (GameObject tutorialObject in tutorialObjects)
			{
				tutorialObject.SetActive(false);
			}
		}

		protected override void OnShown()
		{
			skipTutorialButton.onClick.AddListener(HandleSkipTutorial);

			Time.timeScale = 0f;

			ActivateTutorialObject(_currentTutorialIndex);

			gameCanvas.Hide();
		}

		protected override void OnHidden()
		{
			skipTutorialButton.onClick.RemoveListener(HandleSkipTutorial);

			Time.timeScale = 1f;
		}

		private void HandleSkipTutorial()
		{
			if (_currentTutorialIndex < tutorialObjects.Length - 1)
			{
				DeactivateTutorialObject(_currentTutorialIndex);

				_currentTutorialIndex++;

				ActivateTutorialObject(_currentTutorialIndex);
			}
			else
			{
				DeactivateTutorialObject(_currentTutorialIndex);
				CompleteTutorial();
			}
		}

		private void ActivateTutorialObject(int index)
		{
			if (tutorialObjects.Length == 0) return;
			if (index >= 0 && index < tutorialObjects.Length)
				tutorialObjects[index].SetActive(true);
		}

		private void DeactivateTutorialObject(int index)
		{
			if (index >= 0 && index < tutorialObjects.Length)
			{
				tutorialObjects[index].SetActive(false);
			}
		}

		private void CompleteTutorial()
		{
			gameCanvas.Show();

			Hide();
		}
	}
}
