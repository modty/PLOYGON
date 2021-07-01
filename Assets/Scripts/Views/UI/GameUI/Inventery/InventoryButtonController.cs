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
    private InventoryController _parentController;

    public InventoryController ParentController
    {
        get => _parentController;
        set => _parentController = value;
    }

    private int _index;

    public int Index
    {
        get => _index;
        set => _index = value;
    }
    public Image Icon => icon;
    private ItemInGame _itemInGame;
    
    public ItemInGame ItemInGame
    {
        get => _itemInGame;
        set => SwapItem(value);
    }

    private void SwapItem(ItemInGame fromItem)
    {
        _itemInGame = fromItem;
        if (fromItem != null) ItemShow();
        else
        {
            _parentController.ItemInGame.ContainItems[_index] = null;
            ItemUnShow();
        }
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
                _parentController.SwapItem(_index,target._index);
                target.ItemInGame = _parentController.ItemInGame.ContainItems[target._index];
                ItemInGame = _parentController.ItemInGame.ContainItems[_index];
            }
            // 拖拽物品到其他背包
            else if(obj.tag.Equals("BagBarSlot"))
            {
                BagBarButtonController targetBag=obj.GetComponent<BagBarButtonController>();
                if (targetBag.ItemInGame.AddContainItem(_itemInGame))
                {
                    ItemInGame = null;
                }
                
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
    
    /// <summary>
    /// 使用当前物品
    /// </summary>
    public void ItemUse()
    {
        // 当前物品不能为空
        if (ItemInGame != null)
        {
            // 根据不同物品类型调用使用方法
            switch (ItemUtil.GetItemType(ItemInGame.Uid))
            {
                case 2:
                    // 数量大于0
                    if (ItemInGame.StackCount > 0)
                    {
                        // 尝试使用，需要传入使用目标。由于
                        if (((ConsumableInGame) ItemInGame.Item).Use(_parentController.Player))
                        {
                            ItemInGame.StackCount -= 1;
                            if (ItemInGame.StackCount <= 0)
                            {
                                ItemInGame = null;
                                ItemUnShow();
                            }
                            else
                            {
                                num.text = ItemInGame.StackCount.ToString();
                            }
                        }
                    }
                    break;
            }
            MesPlaneController.Instance.CloseItemMes();
        }
    }
}
