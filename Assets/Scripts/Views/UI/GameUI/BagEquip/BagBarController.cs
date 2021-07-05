using Domain.Data.GameData;
using Scripts;
using UnityEngine;

public class BagBarController : MonoBehaviour
{

    /// <summary>
    /// 背包显示UI板
    /// </summary>

    private GameObject bagBarSlotPrefab;

    [SerializeField] private GameObject bagBarSlots;

    private GDChaPlayer _gdChaPlayer;

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

    private GDEquBackpack[] _backpacks;
    
    private bool[] isEquiped;

    private bool isLoadInventory;

    public BagBarButtonController[] Bags
    {
        get => bags;
        set => bags = value;
    }

    public GDEquBackpack[] Backpacks
    {
        get => _backpacks;
        set => _backpacks = value;
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

    public void SetPlayer(GDChaPlayer gdChaPlayer)
    {
        _gdChaPlayer = gdChaPlayer;
        int length = gdChaPlayer.BagBarShortCutItems.Length;
        bags=new BagBarButtonController[length];
        _backpacks=new GDEquBackpack[length];
        isEquiped=new bool[length];
        for (int i = 0; i < length; i++)
        {
            if (gdChaPlayer.BagBarShortCutItems[i] != null)
            {
                isEquiped[i] = true;
                _backpacks[i] = gdChaPlayer.BagBarShortCutItems[i] as GDEquBackpack;
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
                bags[i].Icon.sprite = _backpacks[i].Icon;
                bags[i].Icon.enabled=true;
                bags[i].Index = i;
                bags[i].Backpack = _backpacks[i];
                bags[i].ParentController = this;
                if (!isLoadInventory)
                {
                    // 加载不显示
                    InventoryController.Instance.LoadInventory(_backpacks[i]);
                    isLoadInventory = true;
                }
            }
        }
    }
    
    
}
