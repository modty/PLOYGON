using System;
using System.Collections;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Data;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Managers;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    private PlayerData _player;
    private CombatUIController _combatUIController;
    private Messenger _messenger;
    public PlayerData ControlledControlledChaState
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
        _messenger = new Messenger();
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
        _messenger.Subscribe<MGameData>(Constants_Event.ControlledCharacter, (message) =>
        {
            _player=message.GameData as PlayerData;
        });
    }
}
