using System;
using ActionPool;
using Commons;
using Data;
using Tools;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// 管理角色的数据（非UnityEngine数据），游戏对象都必须添加此组件
    /// </summary>
    public class DataController:MonoBehaviour
    {
        private GameData _gameData;
        [SerializeField] private TypedGameData dataType;
        [SerializeField] private TypedInteract interactType; 
        public GameData GameData
        {
            get => _gameData;
            set => _gameData = value;
        }

        private void Awake()
        {
            if(dataType.Equals(TypedGameData.Player)) return;
            _gameData = AttributeFactory.CreateAssetMenuAttribute(dataType,gameObject);
            PlayerAttribute _attribute = _gameData as PlayerAttribute;
            if (_attribute != null)
            {
                _attribute.AttackDamage = 102;
                _attribute.AbilityPower = 124;
                _attribute.ArmorResistance = 45;
                _attribute.MagicResistance = 80;
                _attribute.CriticalStrike = 45;
                _attribute.AttackSpeed = 1.2f;
                _attribute.ONHitEffects = 0;
                _attribute.ArmorPenetration = 12;
                _attribute.MagicPenetration = 10;
                _attribute.HealthRegeneration = 50;
                _attribute.MagicRegeneration = 50;
                _attribute.AbilityHaste = 50;
                _attribute.Movement = 450;
                _attribute.LifeSteal = 30;
                _attribute.Ominivamp = 30;
                _attribute.HealthRegon = 7;
                _attribute.ResourceRegon = 4;
                _attribute.Tenacity = 30;
            }
           
            _gameData.TypedInteract = interactType;
            _gameData.Transform = transform;
            _gameData.Uid = UidTool.Instance.RegistUid();
        }
        

        
    }
}