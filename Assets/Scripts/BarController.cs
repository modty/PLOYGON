using System;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Data;
using JetBrains.Annotations;
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
        private void Awake()
        {
            switch (uiElements)
            {
                case TypedUIElements.PlayerMes:
                    EventCenter.AddListener<GameData>("UIElement:"+TypedUIElements.PlayerMes,SetCharacter);
                    break;
                case TypedUIElements.PlayerTarget:
                    EventCenter.AddListener<GameData>("UIElement:"+TypedUIElements.PlayerTarget,SetCharacter);
                    break;
            }
        }

        public void SetCharacter(GameData gameData)
        {
            PlayerAttribute characterAttribute = gameData as PlayerAttribute;
            if(characterAttribute==null) return;
            if (_player != null && !_player.Uid.Equals(characterAttribute.Uid))
            {
            }
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