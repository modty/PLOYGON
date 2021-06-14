using ActionPool;
using Commons;
using Data;

namespace Scripts
{
    public class AAttackSpeed
    {
        private float _baseValue;
        private float _currentValue;
        private float _maxValue;

        private GameData _gameData;

        public GameData GameData
        {
            get => _gameData;
            set => _gameData = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseValue">攻速上限</param>
        public AAttackSpeed(float maxValue)
        {
            _maxValue = maxValue;
        }

        /// <summary>
        /// 更新当前生命值（受伤害）
        /// </summary>
        /// <param name="value"></param>
        public void UpdateCurrentValue(float value)
        {
            if (value < -_currentValue)
            {
                _currentValue = 0;
            }
            else if(_currentValue+value<=_maxValue)
            {
                _currentValue += value;
            }
            else
            {
                _currentValue = _maxValue;
            }
            if (value != 0)
            {
                EventCenter.Broadcast(Constants_Event.AttributeChange+":"+_gameData.Uid+":"+TypedAttribute.AttackSpeed,value);
            }
        }
    }
}