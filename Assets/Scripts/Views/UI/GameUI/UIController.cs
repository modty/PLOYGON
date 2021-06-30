using System;
using System.Collections;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Data;
using Domain.MessageEntities;
using Items;
using Loxodon.Framework.Messaging;
using Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]private GameObject pointMes;
    [SerializeField]private GameObject stat;
    [SerializeField]private GameObject inventory;
    [SerializeField]private GameObject bagBar;
    [SerializeField]private GameObject targetFrame;
    [SerializeField]private GameObject selectPlane;
    [SerializeField]private Camera mainCam;
    [SerializeField] private GameObject mesPlane;
    private GameData _targetGameData;
    private PlayerData _playerGameData;
    private Messenger _messenger;
    private InventoryController _inventory;
    private BagBarController _bagBarController;
    
    public GameObject MesPlane
    {
        get => mesPlane;
        set => mesPlane = value;
    }

    public GameObject PointMes
    {
        get => pointMes;
        set => pointMes = value;
    }

    public GameObject Stat
    {
        get => stat;
        set => stat = value;
    }

    public GameObject Inventory
    {
        get => inventory;
        set => inventory = value;
    }

    public GameObject BagBar
    {
        get => bagBar;
        set => bagBar = value;
    }

    public GameObject TargetFrame
    {
        get => targetFrame;
        set => targetFrame = value;
    }

    public GameObject SelectPlane
    {
        get => selectPlane;
        set => selectPlane = value;
    }

    public Camera MainCam
    {
        get => mainCam;
        set => mainCam = value;
    }

    private void Awake()
    {
        _messenger=Messenger.Default;
        _inventory = GetComponentInChildren<InventoryController>();
        _bagBarController = GetComponentInChildren<BagBarController>();
        RegistSubscribes();
        targetFrame.SetActive(true); 
    }

    private void Start()
    {
        targetFrame.SetActive(false); 
    }

    #region 监听引用

    private ISubscription<MMouseTarget> OnKeyDown_Mouse0_Target;
    private ISubscription<MMouseTarget> OnKeyDown_Mouse0_Walkable;
    private ISubscription<MGameData> OnControlledCharacter_Change;

    #endregion
    /// <summary>
    /// 注册监听
    /// </summary>
    private void RegistSubscribes()
    {
        OnKeyDown_Mouse0_Target=_messenger.Subscribe<MMouseTarget>(TypedInputActions.OnKeyDown_Mouse0_Target.ToString(),(gamedata) =>
        {
            _targetGameData = gamedata.GameData;
            targetFrame.SetActive(true); 
        });
        
        OnKeyDown_Mouse0_Walkable=_messenger.Subscribe<MMouseTarget>(TypedInputActions.OnKeyDown_Mouse0_Walkable.ToString(), (gamedata) =>
        {
            targetFrame.SetActive(false); 
        });

        OnControlledCharacter_Change = _messenger.Subscribe<MGameData>(Constants_Event.ControlledCharacter, (gamedata) =>
        {
            _playerGameData = gamedata.GameData as PlayerData;
        });
    }
}
