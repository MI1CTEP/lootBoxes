using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using MyGame.Cards;

namespace MyGame
{
    public class VideoPlayerPanel : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private RawImage _leftBorder;
        [SerializeField] private RawImage _rightBorder;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _stopButton;

        private Anim _anim;
        private VideoClip _videoClip;
        private RectTransform _leftBorderRect;
        private RectTransform _rightBorderRect;
        private RectTransform _pauseRect;
        private RectTransform _stopRect;
        private Rect _uvRect;
        private float _uvRectY;
        private bool _isActive;
        private bool _isPause;

        public void Init(BigCard bigCard)
        {
            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(0.3f);

            _leftBorderRect = _leftBorder.GetComponent<RectTransform>();
            _rightBorderRect = _rightBorder.GetComponent<RectTransform>();
            _pauseRect = _pauseButton.GetComponent<RectTransform>();
            _stopRect = _stopButton.GetComponent<RectTransform>();

            _pauseButton.onClick.AddListener(Pause);
            _stopButton.onClick.AddListener(TryHide);
            _stopButton.onClick.AddListener(bigCard.StopVideo);

            gameObject.SetActive(false);
        }

        public void Show(Card card)
        {
            gameObject.SetActive(true);
            ResetPanel();
            TryUnloadVideo();
            AsyncContent.LoadVideo(card, (videoClip) => 
            {
                _videoClip = videoClip;
                _videoPlayer.clip = _videoClip;
            });
            _isActive = true;

            _anim.SetNewSequence();
            _anim.Insert(0, _leftBorderRect.DOAnchorPosX(42, 1));
            _anim.Insert(0, _rightBorderRect.DOAnchorPosX(-42, 1));
            _anim.Insert(0, _pauseRect.DOAnchorPosY(70, 1));
            _anim.Insert(0, _stopRect.DOAnchorPosY(70, 1));
            _anim.InsertCallback(1, () => 
            {
                _videoPlayer.gameObject.SetActive(true);
                _videoPlayer.Play();
            });
        }

        public void TryHide()
        {
            if (!_isActive) return;

            _anim.SetNewSequence();
            _anim.Insert(0, _leftBorderRect.DOAnchorPosX(-42, 1));
            _anim.Insert(0, _rightBorderRect.DOAnchorPosX(42, 1));
            _anim.Insert(0, _pauseRect.DOAnchorPosY(-70, 1));
            _anim.Insert(0, _stopRect.DOAnchorPosY(-70, 1));
            _anim.InsertCallback(1, () =>
            {
                _isActive = false;
                _videoPlayer.gameObject.SetActive(false);
                _videoPlayer.Stop();
                TryUnloadVideo();
            });
        }

        private void Pause()
        {
            if (_isPause)
                _videoPlayer.Play();
            else
                _videoPlayer.Pause();
            _isPause = !_isPause;
        }

        private void ResetPanel()
        {
            _isPause = false;
            _videoPlayer.gameObject.SetActive(false);
            _uvRectY = 0;
            _leftBorderRect.anchoredPosition = new Vector2(-42, _leftBorderRect.anchoredPosition.y);
            _rightBorderRect.anchoredPosition = new Vector2(42, _rightBorderRect.anchoredPosition.y);
            _pauseRect.anchoredPosition = new Vector2(_pauseRect.anchoredPosition.x, -70);
            _stopRect.anchoredPosition = new Vector2(_stopRect.anchoredPosition.x, -70);
        }

        private void TryUnloadVideo()
        {
            if(_videoClip != null)
            {
                _videoPlayer.clip = null;
                AsyncContent.ReleaseVideo(_videoClip);
                _videoClip = null;
            }
        }

        private void Update()
        {
            if (!_isActive || _isPause) return;

            _uvRectY += Time.deltaTime;

            _uvRect = _leftBorder.uvRect;
            _uvRect.y = _uvRectY;
            _leftBorder.uvRect = _uvRect;

            _uvRect = _leftBorder.uvRect;
            _uvRect.y = _uvRectY;
            _leftBorder.uvRect = _uvRect;
            _rightBorder.uvRect = _uvRect;
        }
    }
}