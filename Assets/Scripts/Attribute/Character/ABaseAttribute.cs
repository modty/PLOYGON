using Data;
using Domain.Data.GameData;
using Loxodon.Framework.Messaging;
using UnityEngine;

namespace Scripts
{
    public abstract class ABaseAttribute
    {
        protected float _baseValue;
        protected float _currentValue;
        protected float _maxValue;
        protected Messenger _messenger;

        protected ABaseAttribute()
        {
            _messenger=Messenger.Default;;
        }

        public float BaseValue
        {
            get => _baseValue;
            set => _baseValue = value;
        }

        public float CurrentValue
        {
            get => _currentValue;
            set => _currentValue = value;
        }

        public float MaxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }
        protected GDCharacter _gameData;

        public GDCharacter GameData
        {
            get => _gameData;
            set => _gameData = value;
        }

        public abstract void UpdateCurrentValue(float value);
        public abstract void UpdateBaseValue(float value);
        public abstract void UpdateMaxValue(float value);
    }
}