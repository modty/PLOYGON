using System;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Data;
using Domain.MessageEntities;
using JetBrains.Annotations;
using Loxodon.Framework.Messaging;
using Scripts.Commons;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Controller
{
    public class BarController:MonoBehaviour
    {
        public Text text;
        private PlayerAttribute _player;
        public Image bar;
        public TypedAttribute typedAttribute;
        public TypedUIElements uiElements;
        private Messenger _messenger;
        
        private void Awake()
        {
            _messenger=Messenger.Default;
            RegistSubscribes();
        }

        #region 监听引用

        private ISubscription<MGameData> onBarGameDataChange;
        

        #endregion
        /// <summary>
        /// 注册监听
        /// </summary>
        private void RegistSubscribes()
        {
            switch (uiElements)
            {
                case TypedUIElements.PlayerMes:
                    onBarGameDataChange=_messenger.Subscribe<MGameData>(TypedUIElements.PlayerMes.ToString(), (message)=>
                    {
                        SetCharacter(message.GameData);
                    });
                    break;
                case TypedUIElements.PlayerTarget:
                    onBarGameDataChange=_messenger.Subscribe<MGameData>(TypedUIElements.PlayerTarget.ToString(), (message)=>
                    {
                        SetCharacter(message.GameData);
                    });
                    break;
            }
        }
        public void SetCharacter(GameData gameData)
        {
            Debug.Log("设置目标："+gameData.Uid);
            PlayerAttribute characterAttribute = gameData as PlayerAttribute;
            if(characterAttribute==null) return;
            _player = characterAttribute;
            switch (typedAttribute)
            {
                case TypedAttribute.Health:
                    EventCenter.AddListener(Constants_Event.AttributeChange+":"+_player.Uid+":"+typedAttribute,UpdateBar);
                    UpdateBar(_player.Health);
                    break;
                case TypedAttribute.Mana:
                    break;
            }
        }

        public void UpdateBar()
        {
            if(_player==null) return;
            IAttribute ia = null;
            switch (typedAttribute)
            {
                case TypedAttribute.Health:
                    ia = _player.Health;
                    break;
                case TypedAttribute.Mana:
                    break;
            }
            if(ia==null) return;
            float fillAmount = ia.CurrentValue() / ia.MaxValue();
            text.text = (int)ia.CurrentValue()+"/"+(int)ia.MaxValue();
            bar.fillAmount = fillAmount;
        }
        public void UpdateBar(IAttribute ia)
        {
            if(_player==null||ia==null) return;
          
            float fillAmount = ia.CurrentValue() / ia.MaxValue();
            text.text = (int)ia.CurrentValue()+"/"+(int)ia.MaxValue();
            bar.fillAmount = fillAmount;
        }
    }
}