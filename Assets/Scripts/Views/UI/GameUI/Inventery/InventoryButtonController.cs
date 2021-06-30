using Items;
using Scripts;
using Scripts.Commons.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButtonController:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Text num;
    private PlayerData _player;

    public PlayerData Player
    {
        get => _player;
        set
        {
            _player = value;
        }
    }

    public Image Icon => icon;
    private ItemInGame itemInGame;
    
    public ItemInGame ItemInGame
    {
        get => itemInGame;
        set => SwapItem(value);
    }

    private void SwapItem(ItemInGame fromItem)
    {
        itemInGame = fromItem;
        if (fromItem != null) ItemShow();
        else ItemUnShow();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemInGame != null)
        {
           MesPlaneController.Instance.ShowItemMes(string.Format("<color=>"+ItemInGame.TypeName+"</color>"));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemInGame!=null)
        {
            MesPlaneController.Instance.CloseItemMes();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        MesPlaneController.Instance.CalculatePointIconPlanePosition();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            // 在本背包中交换
            if (obj.tag.Equals("InventorySlot"))
            {
                InventoryButtonController target = obj.GetComponent<InventoryButtonController>();
                ItemInGame temp = target.ItemInGame;
                target.ItemInGame = ItemInGame;
                ItemInGame = temp;
            }
            // 拖拽物品到其他背包
            else if(obj.tag.Equals("BagBarSlot"))
            {
            
            }
            // 将物品拖拽到快捷栏
            else if (obj.tag.Equals("ShortcutSlot"))
            {
                ShortCutButtonController target = obj.GetComponent<ShortCutButtonController>();
                ItemInGame temp = target.ItemInGame;
                target.ItemInGame = ItemInGame;
                ItemInGame = temp;
            }
        }
        MesPlaneController.Instance.PointIconClose();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemInGame!=null)
        {
            MesPlaneController.Instance.PointIconShow(ItemInGame,GetComponent<RectTransform>().sizeDelta);
        }
    }

    private void ItemShow()
    {
        Icon.sprite = ItemInGame.Icon;
        Icon.enabled = true;
        if (ItemInGame.StackCount > 1)
        {
            num.text = ItemInGame.StackCount.ToString();
            num.enabled=true;
        }
        else
        {
            num.text = 1.ToString();
            num.enabled=false;
        }
    }

    private void ItemUnShow()
    {
        Icon.enabled = false;
        num.enabled = false;
    }

    public void ItemUse()
    {
        if (itemInGame != null)
        {
            switch (ItemUtil.GetItemType(ItemInGame.Uid))
            {
                case 2:
                    if (itemInGame.StackCount > 0)
                    {
                        if (((ConsumableInGame) itemInGame.Item).Use(_player))
                        {
                            itemInGame.StackCount -= 1;
                            if (itemInGame.StackCount <= 0)
                            {
                                itemInGame = null;
                                ItemUnShow();
                            }
                            else
                            {
                                num.text = itemInGame.StackCount.ToString();
                            }
                        }
                    }
                    break;
            }
            MesPlaneController.Instance.CloseItemMes();
        }
    }
}
