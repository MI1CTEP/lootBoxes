using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;

namespace MyGame
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private RectTransform _testRect;
        [SerializeField] private Settings _settings;
        [SerializeField] private Shop _shop;
        [SerializeField] private MiniCardsController _miniCardsController;
        [SerializeField] private UpPanel _upPanel;
        [SerializeField] private CardPanel _cardPanel;
        [SerializeField] private MainPanel _mainPanel;
        [SerializeField] private ValueParticlesController _valueParticlesController;

        private void Start()
        {
            //Делаем отступ сверху, чтобы закругления экрана и камера не перекрывали интерфейс
            Rect saveArea = Screen.safeArea;
            Vector2 anchorMax = saveArea.position + saveArea.size;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            _testRect.anchorMax = anchorMax;

            Input.multiTouchEnabled = false;
            Application.targetFrameRate = int.MaxValue;
            if (GameData.IsFirstLaunch)
                GameData.Keys.Add(1000, false);

            _settings.Init();
            _shop.Init();
            CardsData.Load();
            _upPanel.Init();
            _cardPanel.Init(_miniCardsController);
            _mainPanel.Init(_cardPanel, _miniCardsController);
            _valueParticlesController.Init();
            GameData.SetNoFirstLaunch();
        }
    }
}