using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

namespace MyGame
{
    public class Anim : MonoBehaviour
    {
        private Sequence _seq;
        private float _timeAnim = 1;

        public void SetTimeAnim(float timeAnim)
        {
            if (timeAnim > 0.01f)
                _timeAnim = timeAnim;
            else
                _timeAnim = 0.01f;
        }

        public void SetNewSequence()
        {
            TryStopAnim();
            _seq = DOTween.Sequence();
            _seq.timeScale = 1 / _timeAnim;
        }

        public void Insert(float startTime, Tween tween) => _seq.Insert(startTime, tween);

        public void InsertToFloat(float startTime, float startValue, float endValue, float timeAnim, DOSetter<float> onChangeValue)
        {
            _seq.Insert(startTime, DOTween.To(onChangeValue, startValue, endValue, timeAnim));
        }

        public void InsertCallback(float startTime, TweenCallback tweenCallback) => _seq.InsertCallback(startTime, tweenCallback);

        private void TryStopAnim()
        {
            if(_seq != null)
            {
                _seq.Kill();
                _seq = null;
            }
        }

        private void OnDestroy()
        {
            TryStopAnim();
        }
    }
}