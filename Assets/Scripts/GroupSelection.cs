using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public sealed class GroupSelection : MonoBehaviour
    {
        [SerializeField] private Text _idText;
        [SerializeField] private Image _outline;
        [SerializeField] private Color _passiveColor;
        [SerializeField] private Color _activeColor;

        private MainPanel _mainPanel;
        private Button _button;
        private int _id;

        public void Init(MainPanel mainPanel, int id)
        {
            _mainPanel = mainPanel;
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(Choose);
            _id = id;
            _idText.text = (id + 1).ToString();
            ResetSelection();
        }

        public void Choose()
        {
            if (GroupSelectionPanel.CurrentGroupSelection == this)
                return;

            if (GroupSelectionPanel.CurrentGroupSelection != null)
                GroupSelectionPanel.CurrentGroupSelection.ResetSelection();


            bool toLeft = false;
            if (_id < GameData.CurrentCardGroupId)
                toLeft = true;
            GameData.CurrentCardGroupId = _id;

            SetSelection();

            _mainPanel.SwitchGropAnim(toLeft);
        }

        public void SetSelection()
        {
            GroupSelectionPanel.CurrentGroupSelection = this;
            transform.localScale = Vector3.one * 1.1f;
            _idText.color = _activeColor;
            _outline.color = _activeColor;
        }

        private void ResetSelection()
        {
            transform.localScale = Vector3.one;
            _idText.color = _passiveColor;
            _outline.color = _passiveColor;
        }
    }
}