using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;
using MyGame.LootBox;
using DG.Tweening;

namespace MyGame
{
    public sealed class MainPanel : MonoBehaviour
    {
        [SerializeField] private SimpleLootBox _simpleLootBox;
        [SerializeField] private GoldLootBox _goldLootBox;
        [SerializeField] private GroupSelectionPanel _groupSelectionPanel;
        [SerializeField] private CardsGroupPanel _cardsGroupPanel;
        [SerializeField] private PassiveIncomePanel _passiveIncomePanel;
        [SerializeField] private MiniCardsPanel _miniCardsPanel;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

        private Anim _showHideAnim;
        private Anim _swithGroupAnim;
        private bool _isActive;

        public static MainPanel Instance { get; set; }

        public void Init(CardPanel cardPanel, MiniCardsController miniCardsController)
        {
            _simpleLootBox.Init(cardPanel);
            _goldLootBox.Init(cardPanel);
            _groupSelectionPanel.Init(this);
            _cardsGroupPanel.Init();
            _cardsGroupPanel.UpdateGroup();
            _passiveIncomePanel.Init();
            _miniCardsPanel.Init(miniCardsController, cardPanel);

            _showHideAnim = gameObject.AddComponent<Anim>();
            _showHideAnim.SetTimeAnim(0.3f);

            _swithGroupAnim = gameObject.AddComponent<Anim>();
            _swithGroupAnim.SetTimeAnim(0.5f);

            _isActive = true;

            Instance = this;
        }

        public void UpdatePanel()
        {
            _cardsGroupPanel.UpdateGroup();
            _passiveIncomePanel.UpdatePanel();
            _miniCardsPanel.UpdateContent();
            _groupSelectionPanel.SetCurrentGroup();
        }

        public void Show()
        {
            if (_isActive) return;

            _isActive = true;
            _showHideAnim.SetNewSequence();
            _showHideAnim.Insert(0, transform.DOScale(Vector3.one, 1));
        }

        public void Hide()
        {
            if (!_isActive) return;

            _isActive = false;
            _showHideAnim.SetNewSequence();
            _showHideAnim.Insert(0, transform.DOScale(Vector3.one * 0.9f, 1));
        }

        public void SwitchGropAnim(bool toLeft) 
        {
            int side = 1;
            if (toLeft) side = -1;

            _verticalLayoutGroup.enabled = false;
            _swithGroupAnim.SetNewSequence();
            _swithGroupAnim.Insert(0, _cardsGroupPanel.transform.DOLocalMoveX(-1080 * side, 0.5f));
            _swithGroupAnim.Insert(0, _passiveIncomePanel.transform.DOLocalMoveX(-1080 * side, 0.5f));
            _swithGroupAnim.Insert(0, _miniCardsPanel.transform.DOLocalMoveX(-1080 * side, 0.5f));
            _swithGroupAnim.InsertCallback(0.5f, () => 
            {
                Vector3 startPosition = _cardsGroupPanel.transform.localPosition;
                startPosition.x = 1080 * side;
                _cardsGroupPanel.transform.localPosition = startPosition;

                startPosition = _miniCardsPanel.transform.localPosition;
                startPosition.x = 1080 * side;
                _miniCardsPanel.transform.localPosition = startPosition;

                startPosition = _passiveIncomePanel.transform.localPosition;
                startPosition.x = 1080 * side;
                _passiveIncomePanel.transform.localPosition = startPosition;

                UpdatePanel();
            });
            _swithGroupAnim.Insert(0.5f, _cardsGroupPanel.transform.DOLocalMoveX(0, 0.5f));
            _swithGroupAnim.Insert(0.5f, _passiveIncomePanel.transform.DOLocalMoveX(0, 0.5f));
            _swithGroupAnim.Insert(0.5f, _miniCardsPanel.transform.DOLocalMoveX(0, 0.5f));
            _swithGroupAnim.InsertCallback(1, () => _verticalLayoutGroup.enabled = true);
        }
    }
}