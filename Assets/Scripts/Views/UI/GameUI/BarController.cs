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
        private GDChaPlayer _gdChaPlayer;
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
                    onBarGameDataChange=_messenger.Subscribe<MGameData>(Constants_Event.ControlledCharacter, (message)=>
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
        public void SetCharacter(GDCharacter gdCharacter)
        {
            GDChaPlayer character = gdCharacter as GDChaPlayer;
            if(character==null) return;
            _gdChaPlayer = character;
            switch (typedAttribute)
            {
                case TypedAttribute.Health:
                    EventCenter.AddListener(Constants_Event.AttributeChange+":"+_gdChaPlayer.Uid+":"+typedAttribute,UpdateBar);
                    UpdateBar(_gdChaPlayer.GetAttribute<AHealth>(TypedAttribute.Health));
                    break;
                case TypedAttribute.Mana:
                    break;
            }
        }

        public void UpdateBar()
        {
            if(_gdChaPlayer==null) return;
            ABaseAttribute ia = null;
            switch (typedAttribute)
            {
                case TypedAttribute.Health:
                    ia = _gdChaPlayer.GetAttribute<AHealth>(TypedAttribute.Health);
                    break;
                case TypedAttribute.Mana:
                    break;
            }
            if(ia==null) return;
            float fillAmount = ia.CurrentValue / ia.MaxValue;
            text.text = (int)ia.CurrentValue+"/"+(int)ia.MaxValue;
            bar.fillAmount = fillAmount;
        }
        public void UpdateBar(ABaseAttribute ia)
        {
            if(_gdChaPlayer==null||ia==null) return;
          
            float fillAmount = ia.CurrentValue / ia.MaxValue;
            text.text = (int)ia.CurrentValue+"/"+(int)ia.MaxValue;
            bar.fillAmount = fillAmount;
        }
    }
}