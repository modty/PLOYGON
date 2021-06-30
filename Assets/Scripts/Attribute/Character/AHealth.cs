using System;
using ActionPool;
using Commons;
using Data;
using Domain.MessageEntities;
using Managers;
using UnityEngine;

namespace Scripts
{
    public class AHealth:ABaseAttribute
    {
        private MCombatTextCreate _mCombatTextCreate;
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
            if (value != 0)
            {
                EventCenter.Broadcast(Constants_Event.AttributeChange+":"+_gameData.Uid+":"+TypedAttribute.Health);
                _mCombatTextCreate.Crit=false;
                _mCombatTextCreate.Direction = (DateTime.Now.ToUniversalTime().Ticks%2)==0;
                Vector3 postion = _gameData.Transform.position;
                postion.y = .4f;
                _mCombatTextCreate.Position = postion;
                _mCombatTextCreate.Text = value.ToString();
                if (value > 0)
                {
                    _mCombatTextCreate.Type = SCTTYPE.Heal;
                }
                else
                {
                    _mCombatTextCreate.Type = SCTTYPE.HealDamage;
                }
                _messenger.Publish(Constants_Event.CombatTextCreate,_mCombatTextCreate);
            }
        }

        public override void UpdateBaseValue(float value)
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

        public override void UpdateMaxValue(float value)
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
        /// 初始化生命值
        /// </summary>
        /// <param name="value"></param>
        public AHealth(int value)
        {
            _baseValue = value;
            _maxValue = value;
            _currentValue = value;
            _mCombatTextCreate = new MCombatTextCreate(this);
        }
    }
}