using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.ScriptableObjects.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Canvases
{
    public class LoseCanvasHandler : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LevelDataSo levelData;
        [SerializeField] private CoinDataSo coinData;

        [Header("Coin Counter")]
        [SerializeField] private Text coinCounterText;
        
        [Header("References")]
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button restartButton;

        private void OnEnable()
        {
            restartButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.Restart
                });
            });

            mainMenuButton.onClick.AddListener(() =>
            {
                EventBus<UIEvents.UIButtonClicked>.Raise(new UIEvents.UIButtonClicked
                {
                    ButtonType = UiButtonType.MainMenu
                });
            });
        }

        private void OnDisable()
        {
            restartButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.RemoveAllListeners();
        }

        private void Update()
        {
            coinCounterText.text = coinData.CoinAmount.ToString();
        }
    }
}