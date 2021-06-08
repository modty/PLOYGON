using System;
using ActionPool;
using Commons;
using Data;
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
            _gameData = AttributeFactory.CreateAssetMenuAttribute(dataType,gameObject);
            _gameData.TypedInteract = interactType;
            _gameData.Transform = transform;
        }
        

        
    }
}