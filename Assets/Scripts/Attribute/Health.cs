using ActionPool;
using Commons;
using Data;

namespace Scripts
{
    public class Health:IAttribute
    {
        // 基础生命值
        private int _baseValue;
        // 最大生命值
        private int _maxValue;
        // 当前生命值
        private int _currentValue;

        private GameData _gameData;

        public GameData GameData
        {
            get => _gameData;
            set => _gameData = value;
        }

        /// <summary>
        /// 初始化生命值
        /// </summary>
        /// <param name="value"></param>
        public Health(int value)
        {
            _baseValue = value;
            _maxValue = value;
            _currentValue = value;
        }
        /// <summary>
        /// 更改基础生命值（升级、降级）
        /// </summary>
        /// <param name="value"></param>
        public void UpdateBaseValue(int value)
        {
            //生命值增加
            if (value > 0)
            {
                // 基础生命值增加时，增加其余属性
                _baseValue += value;
                _maxValue += value;
                _currentValue += value;
            }
            // 生命值减到0以下
            else if(value<-_baseValue)
            {
                _baseValue -= _baseValue;
                _maxValue -= _baseValue;
                _currentValue -= _baseValue;
            }
            else
            {
                _baseValue = value;
                _maxValue -= value;
                _currentValue -= value;
            }
            
            if (value != 0)
            {
                EventCenter.Broadcast(Constants_Event.AttributeChange+":"+_gameData.Uid+":"+TypedAttribute.Health);
            }
        }

        /// <summary>
        /// 增加生命值上限（装备、技能）
        /// </summary>
        public void UpdateMaxValue(int value)
        {
            // 增加生命上限
            if (value > 0)
            {
                _maxValue -= value;
                _currentValue += value;
            }
            // 生命上限减少，最多将额外生命值减到0
            else if (value < -(_maxValue-_baseValue))
            {
                _maxValue = _baseValue;
                _currentValue = _baseValue;

            }
            else
            {
                _maxValue -= _baseValue;
                _currentValue -= _baseValue;
            }

            if (value != 0)
            {
                EventCenter.Broadcast(Constants_Event.AttributeChange+":"+_gameData.Uid+":"+TypedAttribute.Health);
            }
        }

        /// <summary>
        /// 更新当前生命值（受伤害）
        /// </summary>
        /// <param name="value"></param>
        public void UpdateCurrentValue(int value)
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
                EventCenter.Broadcast(Constants_Event.AttributeChange+":"+_gameData.Uid+":"+TypedAttribute.Health);
            }
        }


        public float MaxValue()
        {
            return _maxValue;
        }

        public float CurrentValue()
        {
            return _currentValue;
        }
    }
}