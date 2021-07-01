using System;
using Items;
using Scripts;
using UnityEngine;

public class InventoryController:MonoBehaviour
{
    private static InventoryController instance;
    public static InventoryController Instance => instance;
    private ItemInGame itemInGame;
    private InventoryButtonController[] inventoryButtons;
    [SerializeField] private GameObject slots;
    private GameObject slotPrefab;
    private PlayerData _player;

    public PlayerData Player
    {
        get => _player;
        set => _player = value;
    }

    public ItemInGame ItemInGame
    {
        get => itemInGame;
        set => itemInGame = value;
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

    public void LoadInventory( ItemInGame itemInGame)
    {
        for (int i = 0; i < slots.transform.childCount; i++) {  
            Destroy (slots.transform.GetChild (i).gameObject);  
        }  
        this.itemInGame = itemInGame;
        ItemInGame[] itemInGames = itemInGame.ContainItems;
        inventoryButtons=new InventoryButtonController[itemInGames.Length];
        for (int i = 0; i < itemInGames.Length; i++)
        {
            GameObject obj=Instantiate(slotPrefab, slots.transform);
            inventoryButtons[i] = obj.GetComponent<InventoryButtonController>();
            inventoryButtons[i].ParentController = this;
            inventoryButtons[i].Index = i;
            if (itemInGames[i] != null&&inventoryButtons[i]!=null)
            {
                inventoryButtons[i].ItemInGame = itemInGames[i];
            }
        }
    }
    
    public void SetPlayer(PlayerData playerData)
    {
        _player = playerData;
        LoadInventory(_player.BagBarShortCutItems[_player.BagOpenIndex]);
    }

    public void SwapItem(int fromIndex, int toIndex)
    {
        ItemInGame[] itemInGames = itemInGame.ContainItems;
        if(fromIndex>itemInGames.Length||toIndex>itemInGames.Length) return;
        ItemInGame temp = itemInGames[fromIndex];
        itemInGames[fromIndex] = itemInGames[toIndex];
        itemInGames[toIndex] = temp;
    }
    public void OpenClose(ItemInGame itemInGame)
    {
        if (ItemInGame.Equals(itemInGame))
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        else if(itemInGame!=null)
        {
            LoadInventory(itemInGame);
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(!gameObject.activeSelf);
            }
        }
    }
    
}
