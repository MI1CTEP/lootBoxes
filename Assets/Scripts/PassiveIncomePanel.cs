using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;

namespace MyGame
{
    public sealed class PassiveIncomePanel : MonoBehaviour
    {
        [SerializeField] private Button _getButton;
        [SerializeField] private Image _progress;
        [SerializeField] private Text _getButtonText;
        [SerializeField] private Text _incomeValueText;
        [SerializeField] private Text _quantityInDayText;

        private int[] _maxTimeToIncome;
        private int[] _timesToIncome;
        private float _time;
        private int _incomeValue;
        private bool _isUpgradingProgress;

        public void Init()
        {
            _getButton.onClick.AddListener(GetIncome);

            _maxTimeToIncome = new int[Settings.CardGroups.Length];
            _timesToIncome = new int[Settings.CardGroups.Length];

            GameData.CardGroupPassiveIncome.Time.OnUpLevel += UpdateMaxTimeToIncome;

            for (int i = 0; i < _timesToIncome.Length; i++)
            {
                UpdateMaxTimeToIncome(i);
                _timesToIncome[i] = _maxTimeToIncome[i];
            }
            UpdatePanel();
        }

        public void UpdatePanel()
        {
            UpdateIncomeValue();
            UpdateIncomeInDay();
            UpdateGetButton();
        }

        private void UpdateIncomeValue()
        {
            _incomeValue = 0;
            List<Card> cards = CardsData.GetGroup(GameData.CurrentCardGroupId);
            for (int i = 0; i < cards.Count; i++)
                _incomeValue += Settings.RankCard[cards[i].rank].priceKey;
            _incomeValueText.text = $"+{_incomeValue}";
        }

        private void UpdateIncomeInDay()
        {
            int value = GameData.CardGroupPassiveIncome.InDay.LoadEarnedToday(GameData.CurrentCardGroupId);
            int max = Settings.Upgrades.income.startQuantity + GameData.CardGroupPassiveIncome.InDay.LoadLevel(GameData.CurrentCardGroupId);
            _quantityInDayText.text = $"ÇÀÐÀÁÎÒÀÍÎ ÐÀÇ <color=#FFA437>{value}/{max}</color>";
            _isUpgradingProgress = value < max;
        }

        private void UpdateMaxTimeToIncome(int id)
        {
            _maxTimeToIncome[id] = Settings.Upgrades.income.startTime - Settings.Upgrades.income.stepTime * GameData.CardGroupPassiveIncome.Time.LoadLevel(id);
            if (_maxTimeToIncome[id] < _timesToIncome[id])
                _timesToIncome[id] = _maxTimeToIncome[id];
        }

        private void UpdateGetButton()
        {
            int id = GameData.CurrentCardGroupId;
            if (!_isUpgradingProgress)
            {
                _getButton.interactable = false;
                _progress.fillAmount = 0;
                _getButtonText.text = "ÍÅÒ ÇÀÐÎÁÎÒÊÀ";
                return;
            }

            if (_timesToIncome[id] <= 0)
            {
                _progress.fillAmount = 1f;
                _getButton.interactable = true;
                _getButtonText.text = "ÇÀÁÐÀÒÜ";
            }
            else
            {
                _progress.fillAmount = 1f - (float)(_timesToIncome[id]) / _maxTimeToIncome[id];
                _getButton.interactable = false;
                _getButtonText.text = _timesToIncome[GameData.CurrentCardGroupId].ToString();
            }
        }

        private void GetIncome()
        {
            GameData.CardGroupPassiveIncome.InDay.AddEarnedToday(GameData.CurrentCardGroupId);
            GameData.CardGroupExperience.Add(GameData.CurrentCardGroupId, _incomeValue);
            GameData.Keys.Add(_incomeValue, true);
            _timesToIncome[GameData.CurrentCardGroupId] = _maxTimeToIncome[GameData.CurrentCardGroupId];
            UpdatePanel();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if(_time >= 1)
            {
                for (int i = 0; i < _timesToIncome.Length; i++)
                    _timesToIncome[i]--;
                _time--;
                UpdateGetButton();
            }
        }

        private void OnDestroy()
        {
            GameData.CardGroupPassiveIncome.Time.OnUpLevel -= UpdateMaxTimeToIncome;
        }
    }
}