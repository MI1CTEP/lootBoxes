using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MyGame
{
    public sealed class ValueParticle : MonoBehaviour
    {
        [SerializeField] private Text _valueText;
        [SerializeField] private Sprite _keySprite;
        [SerializeField] private Sprite _labSprite;
        [SerializeField] private Color _whiteColor;
        [SerializeField] private Color _redColor;
        [SerializeField] private Color _greenColor;
        [SerializeField] private Color _violetColor;
        [SerializeField] private Color _goldColor;

        private ValueParticlesController _valueParticlesController;
        private Anim _anim;
        private Image _image;
        private CanvasGroup _canvasGroup;

        public void Init(ValueParticlesController valueParticlesController)
        {
            _valueParticlesController = valueParticlesController;
            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(1);
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show(TypeValueParticle typeValueParticle, int value)
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;

            if(typeValueParticle == TypeValueParticle.Key)
            {
                _image.sprite = _keySprite;
                _image.color = _violetColor;
            }
            else if(typeValueParticle == TypeValueParticle.Lab)
            {
                _image.sprite = _labSprite;
                _image.color = _greenColor;
            }
            else if (typeValueParticle == TypeValueParticle.GoldKey)
            {
                _image.sprite = _keySprite;
                _image.color = _goldColor;
            }
            _image.SetNativeSize();

            if(value < 0)
            {
                _valueText.text = value.ToString();
                _valueText.color = _redColor;
            }
            else
            {
                _valueText.text = $"+{value}";
                _valueText.color = _whiteColor;
            }

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0;
            transform.localPosition = mousePosition;

            _anim.SetNewSequence();
            _anim.Insert(0, _canvasGroup.DOFade(0, 1));
            _anim.Insert(0, transform.DOLocalMoveY(transform.localPosition.y + 100, 1));
            _anim.InsertCallback(1, () =>
            {
                gameObject.SetActive(false);
                _valueParticlesController.ReturnValueParticle(this);
            });
        }
    }

    public enum TypeValueParticle
    {
        Key, Lab, GoldKey
    }
}