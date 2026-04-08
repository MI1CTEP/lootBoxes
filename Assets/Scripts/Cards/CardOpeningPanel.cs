using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace MyGame.Cards
{
    public sealed class CardOpeningPanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text _textProgress;
        [SerializeField] private Image _leftOutlineProgress;
        [SerializeField] private Image _rightOutlineProgress;
        [SerializeField] private Image _closedCardProgress;
        [SerializeField] private Image _closedCardLight;

        private Anim _anim;
        private Reward[] _rewards;
        private int _progress;
        private readonly int _requiredClicks = 20;
        private int _currentClick;
        private bool _isBlockClick;

        public UnityAction OnComplete { get; set; }

        public void Init()
        {
            gameObject.SetActive(false);
            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(0.2f);
            _rewards = new Reward[_requiredClicks];
        }

        public void Show(int keysValue, int labsValue, int goldKeyValue)
        {
            _isBlockClick = true;
            gameObject.SetActive(true);
            ResetPanel();
            GenerateReward(keysValue, labsValue, goldKeyValue);
            _anim.SetNewSequence();
            _anim.Insert(0, transform.DOLocalMoveX(0, 1));
            _anim.Insert(0, transform.DORotate(Vector3.zero, 1));
            _anim.InsertCallback(1, () => _isBlockClick = false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isBlockClick)
                return;

            if(_currentClick < _requiredClicks)
            {
                if(_rewards[_currentClick].value > 0)
                {
                    if(_rewards[_currentClick].id == 1)
                        GameData.Keys.Add(_rewards[_currentClick].value, true);
                    else if (_rewards[_currentClick].id == 2)
                        GameData.Labs.Add(_rewards[_currentClick].value, true);
                    else if (_rewards[_currentClick].id == 3)
                        GameData.GoldKeys.Add(_rewards[_currentClick].value, true);
                }

                _progress += 100 / _requiredClicks;
                ShowText();
                PlayClickAnim();
            }
            else
            {
                _isBlockClick = true;
                PlayHideAnim();
            }
            _currentClick++;
        }

        private void GenerateReward(int keysValue, int labsValue, int goldKeyValue)
        {
            for (int i = 0; i < _rewards.Length; i++)
            {
                _rewards[i].value = 0;
                _rewards[i].id = 0;
            }

            for (int i = 0; i < 4; i++)
            {
                _rewards[i].id = 1;
                _rewards[i].value = Random.Range(keysValue / 10, keysValue / 2 + 1);
                keysValue -= _rewards[i].value;
            }
            _rewards[4].id = 1;
            _rewards[4].value = keysValue;

            for (int i = 5; i < 9; i++)
            {
                _rewards[i].id = 2;
                _rewards[i].value = Random.Range(labsValue / 10, labsValue / 2 + 1);
                labsValue -= _rewards[i].value;
            }
            _rewards[9].id = 2;
            _rewards[9].value = labsValue;

            for (int i = 10; i < 14; i++)
            {
                _rewards[i].id = 3;
                _rewards[i].value = Random.Range(goldKeyValue / 10, goldKeyValue / 2 + 1);
                goldKeyValue -= _rewards[i].value;
            }
            _rewards[14].id = 3;
            _rewards[14].value = goldKeyValue;

            for (int i = 0; i < _rewards.Length; i++)
            {
                int r = Random.Range(0, _rewards.Length);
                (_rewards[i], _rewards[r]) = (_rewards[r], _rewards[i]);
            }
        }

        private void PlayHideAnim()
        {
            _anim.SetNewSequence();
            _anim.Insert(0, transform.DORotate(new Vector3(0, 90, 0), 1));
            _anim.InsertCallback(1, () => 
            {
                gameObject.SetActive(false);
                OnComplete?.Invoke();
            });
        }

        private void PlayClickAnim()
        {
            Vector3 randomRotate = Vector3.zero;
            randomRotate.z = Random.Range(-2f, 2f);
            float progress = _progress / 100f;
            _anim.SetNewSequence();
            _anim.Insert(0, _leftOutlineProgress.DOFillAmount(progress, 0.5f));
            _anim.Insert(0, _rightOutlineProgress.DOFillAmount(progress, 0.5f));
            _anim.Insert(0, _closedCardProgress.DOFillAmount(progress, 0.5f));
            _anim.Insert(0, _closedCardLight.DOFade(Mathf.Pow(progress, 2), 1));
            _anim.Insert(0, transform.DOScale(Vector3.one * 0.97f, 0.5f));
            _anim.Insert(0, transform.DORotate(randomRotate, 0.5f));
            _anim.Insert(0.5f, transform.DOScale(Vector3.one, 0.5f));
            _anim.Insert(0.5f, transform.DORotate(Vector3.zero, 0.5f));
        }

        private void ResetPanel()
        {
            _progress = 0;
            _currentClick = 0;

            _leftOutlineProgress.fillAmount = 0;
            _rightOutlineProgress.fillAmount = 0;
            _closedCardProgress.fillAmount = 0;

            Color closedCardLightColor = _closedCardLight.color;
            closedCardLightColor.a = 0;
            _closedCardLight.color = closedCardLightColor;

            transform.localScale = Vector3.one;
            transform.eulerAngles = new Vector3(0, 90, 0);
            transform.localPosition = new Vector3(750, 125, 0);

            ShowText();
        }

        private void ShowText()
        {
            _textProgress.text = $"{_progress}%";
        }

        private struct Reward
        {
            public int value;
            public int id;
        }
    }
}