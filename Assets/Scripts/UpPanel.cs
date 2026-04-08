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

        public void Init()
        {
            UpdateKeyText(0);
            UpdateLabText(0);
            UpdateGoldKeyText(0);
            GameData.Keys.OnAdd += UpdateKeyText;
            GameData.Labs.OnAdd += UpdateLabText;
            GameData.GoldKeys.OnAdd += UpdateGoldKeyText;
        }

        private void UpdateKeyText(int value)
        {
            _textKeyValue.text = GameData.Keys.GetValue().ToString("N0");
            if(GameData.Keys.GetValue() < 500)
            {
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

            _time += Time.deltaTime * 10 / 3;
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
        }
    }
}