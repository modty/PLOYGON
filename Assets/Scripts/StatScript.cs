using System;
using System.Collections;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Data;
using Managers;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

public class StatScript : MonoBehaviour
{
    private PlayerAttribute _player;
    private CombatTextManager _combatTextManager;
    public PlayerAttribute ControlledControlledChaState
    {
        get => _player;
        set => _player = value;
    }

    [SerializeField] private Text attackDamage;
    [SerializeField] private Text abilityPower;
    [SerializeField] private Text armorResistance;
    [SerializeField] private Text magicResistance;
    [SerializeField] private Text attackSpeed;
    [SerializeField] private Text abilityHaste;
    [SerializeField] private Text criticalStrike;
    [SerializeField] private Text movement;

    private void Awake()
    {
        if(_player==null) return;
        EventCenter.AddListener(Constants_Event.AttributeChange+":"+_player.Uid+":"+TypedAttribute.AttackDamage,
            () =>
            {
                attackDamage.text = ((int) _player.AttackDamage.CurrentValue()).ToString();
            });
        EventCenter.AddListener<GameData>("UIElement:"+TypedUIElements.PlayerMes, (gameData) =>
        {
            _player=gameData as PlayerAttribute;
                /*EventCenter.AddListener<float>(Constants_Event.AttributeChange+":"+_player.Uid+":"+TypedAttribute.AttackSpeed,
                    (value) =>
                    {
                
                    });
                EventCenter.AddListener<float>(Constants_Event.AttributeChange+":"+_player.Uid+":"+TypedAttribute.AttackSpeed,
                    (value) =>
                    {
                
                    });
                EventCenter.AddListener<float>(Constants_Event.AttributeChange+":"+_player.Uid+":"+TypedAttribute.AttackSpeed,
                    (value) =>
                    {
                
                    });
                EventCenter.AddListener<float>(Constants_Event.AttributeChange+":"+_player.Uid+":"+TypedAttribute.AttackSpeed,
                    (value) =>
                    {
                
                    });
                EventCenter.AddListener<float>(Constants_Event.AttributeChange+":"+_player.Uid+":"+TypedAttribute.AttackSpeed,
                    (value) =>
                    {
                
                    });
                EventCenter.AddListener<float>(Constants_Event.AttributeChange+":"+_player.Uid+":"+TypedAttribute.AttackSpeed,
                    (value) =>
                    {
                
                    });
                EventCenter.AddListener<float>(Constants_Event.AttributeChange+":"+_player.Uid+":"+TypedAttribute.AttackSpeed,
                    (value) =>
                    {
                
                    });*/
        });
    }
}
