﻿using Data;

namespace Scripts
{
    /// <summary>
    /// 攻击力。计算等操作仅在内部进行，外部不进行计算。
    /// 计算规则：
    ///     由物理攻击力导致的属性计算，在此处进行。
    /// </summary>
    public class AttackDamage:IAttribute
    {

        /// <summary>
        /// 不附加任何Buff的攻击力，基础攻击力
        /// </summary>
        private int _baseValue;
        /// <summary>
        /// 当前用于计算的攻击力，受到所有影响之后的。
        /// </summary>
        private int _currentValue;


        private GameData _gameData;

        public GameData GameData
        {
            get => _gameData;
            set => _gameData = value;
        }

        public float MaxValue()
        {
            return _baseValue;
        }

        public float CurrentValue()
        {
            return _currentValue;
        }

        public AttackDamage(int baseValue)
        {
            _baseValue = baseValue;
            _currentValue = baseValue;
        }
    }
}