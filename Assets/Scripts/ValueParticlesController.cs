using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public sealed class ValueParticlesController : MonoBehaviour
    {
        [SerializeField] private ValueParticle _valueParticlePrefab;

        private readonly Stack<ValueParticle> _valueParticles = new();

        public void Init()
        {
            GameData.Keys.OnParticle += ShowKey;
            GameData.Labs.OnParticle += ShowLab;
            GameData.GoldKeys.OnParticle += ShowGoldKey;
        }

        public void ReturnValueParticle(ValueParticle valueParticle)
        {
            _valueParticles.Push(valueParticle);
        }

        private void ShowKey(int value)
        {
            ValueParticle valueParticle = GetValueParticle();
            valueParticle.Show(TypeValueParticle.Key, value);
        }

        private void ShowLab(int value)
        {
            ValueParticle valueParticle = GetValueParticle();
            valueParticle.Show(TypeValueParticle.Lab, value);
        }

        private void ShowGoldKey(int value)
        {
            ValueParticle valueParticle = GetValueParticle();
            valueParticle.Show(TypeValueParticle.GoldKey, value);
        }

        private ValueParticle GetValueParticle()
        {
            ValueParticle valueParticle;
            if(_valueParticles.Count == 0)
            {
                valueParticle = Instantiate(_valueParticlePrefab, transform);
                valueParticle.Init(this);
            }
            else
            {
                valueParticle = _valueParticles.Pop();
            }
            return valueParticle;
        }

        private void OnDestroy()
        {
            GameData.Keys.OnParticle -= ShowKey;
            GameData.Labs.OnParticle -= ShowLab;
            GameData.GoldKeys.OnParticle -= ShowGoldKey;
        }
    }
}