using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Scripts;
using UnityEngine;

public class BagBarController : MonoBehaviour
{

    /// <summary>
    /// 背包显示UI板
    /// </summary>

    private GameObject bagBarSlotPrefab;

    [SerializeField] private GameObject bagBarSlots;

    private PlayerData _player;

    public GameObject BagBarSlotPrefab
    {
        get => bagBarSlotPrefab;
        set => bagBarSlotPrefab = value;
    }
    private static BagBarController instance;

    public static BagBarController Instance => instance;
    
    /// <summary>
    /// 背包装备栏
    /// </summary>
    private BagBarButtonController[] bags;

    private ItemInGame[] bagDatas;
    
    private bool[] isEquiped;

    private bool isLoadInventory;

    public BagBarButtonController[] Bags
    {
        get => bags;
        set => bags = value;
    }

    public ItemInGame[] BagDatas
    {
        get => bagDatas;
        set => bagDatas = value;
    }

    public bool[] IsEquiped
    {
        get => isEquiped;
        set => isEquiped = value;
    }

    /// <summary>
    /// 加载数据（模拟）
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    public void SetPlayer(PlayerData playerData)
    {
        _player = playerData;
        int length = playerData.BagBarShortCutItems.Length;
        bags=new BagBarButtonController[length];
        bagDatas=new ItemInGame[length];
        isEquiped=new bool[length];
        for (int i = 0; i < length; i++)
        {
            if (playerData.BagBarShortCutItems[i] != null)
            {
                isEquiped[i] = true;
                bagDatas[i] = playerData.BagBarShortCutItems[i];
            }
        }
        LoadBagBar();
    }
    public void LoadBagBar()
    {
        for (int i = 0; i < bags.Length; i++)
        {
            GameObject obj = Instantiate(bagBarSlotPrefab, bagBarSlots.transform);
            bags[i]=obj.GetComponent<BagBarButtonController>();
            if (isEquiped[i]&&bags[i]!=null)
            {
                bags[i].Icon.sprite = bagDatas[i].Icon;
                bags[i].Icon.enabled=true;
                bags[i].ItemInGame = bagDatas[i];
                bags[i].ParentController = this;
                if (!isLoadInventory)
                {
                    // 加载不显示
                    InventoryController.Instance.LoadInventory(bagDatas[i]);
                    isLoadInventory = true;
                }
            }
        }
    }
    
    
}
