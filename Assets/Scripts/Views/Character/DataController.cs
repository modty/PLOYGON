using System;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Data;
using States;
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


        private List<BaseState> _updateList=new List<BaseState>();
        private List<BaseState> _fixedUpdateList=new List<BaseState>();
        public GameData GameData
        {
            get => _gameData;
            set => _gameData = value;
        }

        public List<BaseState> UpdateList
        {
            get => _updateList;
            set => _updateList = value;
        }

        public List<BaseState> FixedUpdateList
        {
            get => _fixedUpdateList;
            set => _fixedUpdateList = value;
        }

        private void Awake()
        {
            if(dataType.Equals(TypedGameData.Player)||dataType != TypedGameData.Floor) return;
            _gameData = DataFactory.CreateAssetMenuAttribute(dataType,gameObject);
            _gameData.TypedInteract = interactType;
            _gameData.Transform = transform;
            _gameData.Uid = UidTool.Instance.RegistUid();
        }

        private void Update()
        {
            if (_updateList != null)
            {
                foreach (BaseState baseState in _updateList)
                {
                    baseState.Update();
                }
            }
            
        }

        private void FixedUpdate()
        {
            if (_fixedUpdateList != null)
            {
                foreach (BaseState baseState in _fixedUpdateList)
                {
                    baseState.Update();
                }
            }
           
        }
    }
}