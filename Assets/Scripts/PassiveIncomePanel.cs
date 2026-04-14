using System;
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
        [SerializeField] private GameObject _newIndicator;

        private int _startDay;
        private int[] _maxTimeToIncome;
        private int[] _timesToIncome;
        private bool[] _isConpleteIncome;
        private float _time;
        private int _incomeValue;
        private bool _isUpgradingProgress;

        public void Init()
        {
            _startDay = DateTime.Now.DayOfYear;

            _getButton.onClick.AddListener(GetIncome);

            _maxTimeToIncome = new int[Settings.CardGroups.Length];
            _timesToIncome = new int[Settings.CardGroups.Length];
            _isConpleteIncome = new bool[Settings.CardGroups.Length];

            GameData.CardGroupPassiveIncome.Time.OnUpLevel += UpdateMaxTimeToIncome;

            for (int i = 0; i < _timesToIncome.Length; i++)
            {
                UpdateMaxTimeToIncome(i);

                int value = GameData.CardGroupPassiveIncome.InDay.LoadEarnedToday(i);
                int max = Settings.Upgrades.income.startQuantity + GameData.CardGroupPassiveIncome.InDay.LoadLevel(i);
                if (value < max)
                    _timesToIncome[i] = _maxTimeToIncome[i];
                else
                    _timesToIncome[i] = int.MaxValue;
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
                _newIndicator.SetActive(false);
                _getButton.interactable = false;
                _progress.fillAmount = 0;
                TimeSpan remainingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1) - DateTime.Now;
                _getButtonText.text = $"{remainingTime.Hours:00}:{remainingTime.Minutes:00}:{remainingTime.Seconds:00}";
                return;
            }

            if (_isConpleteIncome[id])
            {
                _newIndicator.SetActive(true);
                _progress.fillAmount = 1f;
                _getButton.interactable = true;
                _getButtonText.text = "ÇÀÁÐÀÒÜ";
            }
            else
            {
                _newIndicator.SetActive(false);
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

            int value = GameData.CardGroupPassiveIncome.InDay.LoadEarnedToday(GameData.CurrentCardGroupId);
            int max = Settings.Upgrades.income.startQuantity + GameData.CardGroupPassiveIncome.InDay.LoadLevel(GameData.CurrentCardGroupId);
            if (value < max)
                _timesToIncome[GameData.CurrentCardGroupId] = _maxTimeToIncome[GameData.CurrentCardGroupId];
            else
                _timesToIncome[GameData.CurrentCardGroupId] = int.MaxValue;

            GroupSelectionPanel.Instance.AddAndRemoveNewAction(GameData.CurrentCardGroupId, false);
            _isConpleteIncome[GameData.CurrentCardGroupId] = false;
            UpdatePanel();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if(_time >= 1)
            {
                for (int i = 0; i < _timesToIncome.Length; i++)
                {
                    _timesToIncome[i]--;
                    if(_isConpleteIncome[i] == false && _timesToIncome[i] <= 0)
                    {
                        GroupSelectionPanel.Instance.AddAndRemoveNewAction(i, true);
                        _isConpleteIncome[i] = true;
                    }
                }
                _time--;
                UpdateGetButton();

                if(DateTime.Now.DayOfYear != _startDay)
                {
                    for (int i = 0; i < _timesToIncome.Length; i++)
                    {
                        if (_timesToIncome[i] > _maxTimeToIncome[i])
                            _timesToIncome[i] = _maxTimeToIncome[i];
                    }
                    UpdatePanel();
                }
            }
        }

        private void OnDestroy()
        {
            GameData.CardGroupPassiveIncome.Time.OnUpLevel -= UpdateMaxTimeToIncome;
        }
    }
}