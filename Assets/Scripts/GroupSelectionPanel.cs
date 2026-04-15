using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace MyGame
{
    public sealed class GroupSelectionPanel : MonoBehaviour, IPointerMoveHandler
    {
        [SerializeField] private GroupSelection _groupSelectionPrefab;
        [SerializeField] private RectTransform _content;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;

        private GroupSelection[] _groupSelections;
        private RectTransform[] _groupSelectionRects;
        private int[] _newActions;
        private Anim _anim;

        public static GroupSelectionPanel Instance { get; set; }
        public static GroupSelection CurrentGroupSelection { get; set; }

        public void Init(MainPanel mainPanel)
        {
            _groupSelections = new GroupSelection[Settings.CardGroups.Length];
            _groupSelectionRects = new RectTransform[Settings.CardGroups.Length];
            _newActions = new int[Settings.CardGroups.Length];
            for (int i = 0; i < _groupSelections.Length; i++)
            {
                _groupSelections[i] = Instantiate(_groupSelectionPrefab, _content);
                _groupSelections[i].Init(mainPanel, i);
                _groupSelectionRects[i] = _groupSelections[i].GetComponent<RectTransform>();
            }
            _groupSelections[0].SetSelection();

            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(0.2f);

            _leftButton.onClick.AddListener(() => SwitchGroup(-1));
            _rightButton.onClick.AddListener(() => SwitchGroup(1));

            Instance = this;
        }

        public void AddAndRemoveNewAction(int groupId, bool isAdd)
        {
            if (isAdd)
            {
                _newActions[groupId]++;
                _groupSelections[groupId].SetActiveIndicator(true);
            }
            else
            {
                _newActions[groupId]--;
                if(_newActions[groupId] <= 0)
                    _groupSelections[groupId].SetActiveIndicator(false);
            }
        }

        public void SetCurrentGroup()
        {
            float maxAnchoredX = _content.sizeDelta.x - 800;
            float targetAnchoredX = _groupSelectionRects[GameData.CurrentCardGroupId].anchoredPosition.x - 400f;
            targetAnchoredX = Mathf.Clamp(targetAnchoredX, 0, maxAnchoredX);
            _content.anchoredPosition = new Vector2(-targetAnchoredX, _content.anchoredPosition.y);
            _groupSelections[GameData.CurrentCardGroupId].SetCurrent();
        }

        private void SwitchGroup(int id)
        {
            id += GameData.CurrentCardGroupId;
            if (id < 0) id = _groupSelections.Length - 1;
            else if (id >= _groupSelections.Length) id = 0;

            float maxAnchoredX = _content.sizeDelta.x - 800;
            float targetAnchoredX = _groupSelectionRects[id].anchoredPosition.x - 400f;
            targetAnchoredX = Mathf.Clamp(targetAnchoredX, 0, maxAnchoredX);

            _anim.SetNewSequence();
            _anim.Insert(0, _content.DOAnchorPosX(-targetAnchoredX, 1));

            _groupSelections[id].Choose();
        }

        private void OnDestroy()
        {
            CurrentGroupSelection = null;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            _anim.TryStopAnim();
        }
    }
}