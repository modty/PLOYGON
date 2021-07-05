using Domain.Data.GameData;
using Scripts;
using UnityEngine;

public class InventoryController:MonoBehaviour
{
    private static InventoryController instance;
    public static InventoryController Instance => instance;
    private GDEquBackpack _backpackData;
    private InventoryButtonController[] inventoryButtons;
    [SerializeField] private GameObject slots;
    private GameObject slotPrefab;
    private GDChaPlayer _gdChaPlayer;

    public GDChaPlayer GdChaPlayer
    {
        get => _gdChaPlayer;
        set => _gdChaPlayer = value;
    }

    public GDEquBackpack BackpackData
    {
        get => _backpackData;
        set => _backpackData = value;
    }

    public GameObject SlotPrefab
    {
        get => slotPrefab;
        set => slotPrefab = value;
    }

    private void Awake()
    {
        instance = this;
    }

    public void LoadInventory(GDEquBackpack backpackData)
    {
        for (int i = 0; i < slots.transform.childCount; i++) {  
            Destroy (slots.transform.GetChild (i).gameObject);  
        }  
        this._backpackData = backpackData;
        GDBase[] itemInGames = backpackData.ContainData;
        inventoryButtons=new InventoryButtonController[itemInGames.Length];
        for (int i = 0; i < itemInGames.Length; i++)
        {
            GameObject obj=Instantiate(slotPrefab, slots.transform);
            inventoryButtons[i] = obj.GetComponent<InventoryButtonController>();
            inventoryButtons[i].ParentController = this;
            inventoryButtons[i].Index = i;
            if (itemInGames[i] != null&&inventoryButtons[i]!=null)
            {
                inventoryButtons[i].SlotData = itemInGames[i];
            }
        }
    }
    
    public void SetPlayer(GDChaPlayer gdChaPlayer)
    {
        _gdChaPlayer = gdChaPlayer;
        LoadInventory(_gdChaPlayer.BagBarShortCutItems[_gdChaPlayer.BagOpenIndex] as GDEquBackpack);
    }

    public void SwapItem(int fromIndex, int toIndex)
    {
        GDBase[] itemInGames = _backpackData.ContainData;
        if(fromIndex>itemInGames.Length||toIndex>itemInGames.Length) return;
        GDBase temp = itemInGames[fromIndex];
        itemInGames[fromIndex] = itemInGames[toIndex];
        itemInGames[toIndex] = temp;
    }
    public void OpenClose(GDEquBackpack backpack)
    {
        Debug.Log(_backpackData.Equals(backpack));
        if (_backpackData.Equals(backpack))
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        else if(_backpackData!=null)
        {
            LoadInventory(backpack);
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
    }
    
}
