using ActionPool;
using Commons;
using Data;

namespace Scripts
{
    public class AAttackSpeed:ABaseAttribute
    {
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
        public override void UpdateCurrentValue(float value)
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
        }

        public override void UpdateBaseValue(float value)
        {
            
        }

        public override void UpdateMaxValue(float value)
        {
        }

        public  void UpdateCurrentValue()
        {
            
        }
    }
}