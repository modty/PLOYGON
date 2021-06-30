using Data;

namespace Scripts
{
    /// <summary>
    /// 攻击力。计算等操作仅在内部进行，外部不进行计算。
    /// 计算规则：
    ///     由物理攻击力导致的属性计算，在此处进行。
    /// </summary>
    public class AAttackDamage:ABaseAttribute
    {

        public AAttackDamage(int baseValue):base()
        {
            _baseValue = baseValue;
            _currentValue = baseValue;
        }

        public override void UpdateCurrentValue(float value)
        {
            
        }

        public override void UpdateBaseValue(float value)
        {
            
        }

        public override void UpdateMaxValue(float value)
        {
        }
    }
}