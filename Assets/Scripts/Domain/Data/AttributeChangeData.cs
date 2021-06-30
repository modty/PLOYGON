using System.Collections.Generic;
using ActionPool;
using Attribute.Items;
using Commons;
using Data;
using Domain.MessageEntities;
using Loxodon.Framework.Contexts;
using Managers;
using Scripts;
using UnityEngine;

namespace Domain.Data
{
 /// <summary>
    /// 属性改变、恢复类物品，如药水、餐食、药材、丹药等
    /// 注：Buff也可使用此类，如装备增加的回血Buff
   /// </summary>
    public class AttributeChangeData:DataBase
    {
        /// <summary>
        /// 根据此枚举变量判断改变方式
        /// </summary>
        private TypedAttributeChange _typedAttributeChange;

        public TypedAttributeChange TypedAttributeChange
        {
            get => _typedAttributeChange;
            set => _typedAttributeChange = value;
        }

        private MCombatTextCreate _mCombatTextCreate;

        public AttributeChangeData():base()
        {
            _attributes = new List<AttributeChangeEntity>();
            _mCombatTextCreate = new MCombatTextCreate(this);
        }


        /// <summary>
        /// 物品作用的目标上下文。
        /// </summary>
        private GameData _target;

        public GameData Target
        {
            get => _target;
            set => _target = value;
        }
        /// <summary>
        /// 改变的变量
        /// </summary>
        private List<AttributeChangeEntity> _attributes;

        public void AddAttribute(AttributeChangeEntity attributeChangeEntity)
        {
            _attributes.Add(attributeChangeEntity);
        }

        protected override bool CanUse()
        {
            return true;
        }

        protected override void DoUse()
        {
            // 如果目标不能接收本属性变化，直接返回
            if (!_target.CanReceiveUse(this)) return;
            switch (TypedAttributeChange)
            {
                case TypedAttributeChange.Instant:
                    foreach (AttributeChangeEntity attributeChangeEntity in _attributes)
                    {
                        ABaseAttribute attribute=_target.GetAttribute<ABaseAttribute>(attributeChangeEntity.TypedAttribute);

                        attribute.UpdateCurrentValue(attributeChangeEntity.ConstantValue);
                    }
                    break;
                case TypedAttributeChange.Continuous:
                    foreach (AttributeChangeEntity attributeChangeEntity in _attributes)
                    {
                        ABaseAttribute attribute=_target.GetAttribute<ABaseAttribute>(attributeChangeEntity.TypedAttribute);
                        attribute.UpdateCurrentValue(attributeChangeEntity.ConstantValue);
                        if (attributeChangeEntity.Times > 0)
                        {
                            int times=0;
                            Timer _testTimer = null;
                            _testTimer = Timer.Register(
                                duration: attributeChangeEntity.Duration/1000f,
                                () =>
                                {
                                    attribute.UpdateCurrentValue(attributeChangeEntity.Value);
                                    times++;
                                    if (times >= attributeChangeEntity.Times)
                                    {
                                        _testTimer.Cancel();
                                    }
                                },
                                isLooped: true);
                        }
                        
                    }
                    
                    
                    break;
            }
        }

        public override bool CanReceiveUse(DataBase iData)
        {
            return false;
        }
    }

    public class AttributeChangeEntity
    {
        /// <summary>
        /// 改变的属性（如生命值）
        /// </summary>
        private TypedAttribute _typedAttribute;

        private int _constantValue;
        /// <summary>
        /// 每次改变的数值（如100）
        /// </summary>
        private int _value;
        /// <summary>
        /// 改变次数(如2次)
        /// </summary>
        private int _times;
        /// <summary>
        /// 两次间隔的时间，单位：ms（如500ms）
        /// 上诉例子即为：每0.5s恢复100生命值，共恢复两次。
        /// </summary>
        private int _duration;

        public AttributeChangeEntity(TypedAttribute typedAttribute,int constant, int value, int times, int duration)
        {
            _constantValue = constant;
            _typedAttribute = typedAttribute;
            _value = value;
            _times = times;
            _duration = duration;
        }

        public int ConstantValue
        {
            get => _constantValue;
            set => _constantValue = value;
        }

        public TypedAttribute TypedAttribute
        {
            get => _typedAttribute;
            set => _typedAttribute = value;
        }

        public int Value
        {
            get => _value;
            set => _value = value;
        }

        public int Times
        {
            get => _times;
            set => _times = value;
        }

        public int Duration
        {
            get => _duration;
            set => _duration = value;
        }
    }
    public enum TypedAttributeChange
    {
        // 瞬间恢复，次类型时，仅调用AttributeChangeEntity类中的Value值，其余属性不调用。
        Instant,
        Continuous
    }
}