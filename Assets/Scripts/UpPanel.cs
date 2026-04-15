using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public sealed class UpPanel : MonoBehaviour
    {
        [SerializeField] private Text _textKeyValue;
        [SerializeField] private Text _textLabValue;
        [SerializeField] private Text _textGoldKeyValue;
        [SerializeField] private RectTransform _keyRect;
        [SerializeField] private Text _textKeyTimer;

        private float _time;
        private int _maxKey;
        private int _keysPerHour;

        public void Init()
        {
            UpdateKeyText(0);
            UpdateLabText(0);
            UpdateGoldKeyText(0);
            GameData.Keys.OnAdd += UpdateKeyText;
            GameData.Labs.OnAdd += UpdateLabText;
            GameData.GoldKeys.OnAdd += UpdateGoldKeyText;
            GameData.AutoAddKeys.OnActivatePremium += CheckMaxKeys;

            int currentKeys = GameData.Keys.GetValue();
            if (currentKeys < _maxKey)
            {
                int elapsedTime = GameData.AutoAddKeys.LoadElapsedTime();
                int keys = (elapsedTime * _keysPerHour) / 3600;
                if (currentKeys + keys > _maxKey)
                    keys = _maxKey - currentKeys;
                GameData.Keys.Add(keys, false);
            }
        }

        private void UpdateKeyText(int value)
        {
            _textKeyValue.text = GameData.Keys.GetValue().ToString("N0");
            CheckMaxKeys();
        }

        private void CheckMaxKeys()
        {
            _maxKey = Settings.AutoAddKeys.maxKeys;
            _keysPerHour = Settings.AutoAddKeys.keysPerHour;
            if (GameData.AutoAddKeys.IsPremium())
            {
                _maxKey = Settings.AutoAddKeys.maxKeysPremium;
                _keysPerHour = Settings.AutoAddKeys.keysPerHourPremium;
            }

            if (GameData.Keys.GetValue() < _maxKey)
            {
                GameData.AutoAddKeys.SaveLastTime();
                _textKeyTimer.gameObject.SetActive(true);
                _keyRect.sizeDelta = new Vector2(230, 40);
            }
            else
            {
                _textKeyTimer.gameObject.SetActive(false);
                _keyRect.sizeDelta = new Vector2(180, 40);
            }
        }

        private void UpdateLabText(int value)
        {
            _textLabValue.text = GameData.Labs.GetValue().ToString("N0");
        }

        private void UpdateGoldKeyText(int value)
        {
            _textGoldKeyValue.text = GameData.GoldKeys.GetValue().ToString("N0");
        }

        private void Update()
        {
            if (!_textKeyTimer.gameObject.activeSelf) return;

            _time += Time.deltaTime * _keysPerHour / 36f;
            if (_time >= 100)
            {
                GameData.Keys.Add(1, false);
                _time -= 100;
            }

            _textKeyTimer.text = "." + _time.ToString("00");
        }

        private void OnDestroy()
        {
            GameData.Keys.OnAdd -= UpdateKeyText;
            GameData.Labs.OnAdd -= UpdateLabText;
            GameData.GoldKeys.OnAdd -= UpdateGoldKeyText;
            GameData.AutoAddKeys.OnActivatePremium -= CheckMaxKeys;
        }
    }
}