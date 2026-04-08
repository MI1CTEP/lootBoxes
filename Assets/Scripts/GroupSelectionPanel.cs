using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;

namespace MyGame
{
    public sealed class GroupSelectionPanel : MonoBehaviour
    {
        [SerializeField] private GroupSelection _groupSelectionPrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;

        private GroupSelection[] _groupSelections;

        public static GroupSelection CurrentGroupSelection { get; set; }

        public void Init(MainPanel mainPanel)
        {
            _groupSelections = new GroupSelection[Settings.CardGroups.Length];
            for (int i = 0; i < _groupSelections.Length; i++)
            {
                _groupSelections[i] = Instantiate(_groupSelectionPrefab, _content);
                _groupSelections[i].Init(mainPanel, i);
            }
            _groupSelections[0].SetSelection();

            _leftButton.onClick.AddListener(() => SwitchGroup(-1));
            _rightButton.onClick.AddListener(() => SwitchGroup(1));
        }

        private void SwitchGroup(int id)
        {
            id += GameData.CurrentCardGroupId;
            if (id < 0) id = _groupSelections.Length - 1;
            else if (id >= _groupSelections.Length) id = 0;

            _groupSelections[id].Choose();
        }

        private void OnDestroy()
        {
            CurrentGroupSelection = null;
        }
    }
}