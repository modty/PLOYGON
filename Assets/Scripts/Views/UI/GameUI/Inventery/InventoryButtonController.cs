using Domain.Data.GameData;
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
    private GDBase _slotData;
    
    public GDBase SlotData
    {
        get => _slotData;
        set => SwapItem(value);
    }

    private void SwapItem(GDBase fromItem)
    {
        _slotData = fromItem;
        if (fromItem != null) ItemShow();
        else
        {
            _parentController.BackpackData.ContainData[_index] = null;
            ItemUnShow();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_slotData != null)
        {
           MesPlaneController.Instance.ShowItemMes(string.Format("<color=>"+_slotData.Title+"</color>"));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_slotData!=null)
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
                _parentController.SwapItem(_index,target.Index);
                target.SlotData = _parentController.BackpackData.ContainData[target.Index];
                SlotData = _parentController.BackpackData.ContainData[_index];
            }
            // 拖拽物品到其他背包
            else if(obj.tag.Equals("BagBarSlot"))
            {
                BagBarButtonController targetBag=obj.GetComponent<BagBarButtonController>();
                if (targetBag.Backpack.AddContainData(_slotData))
                {
                    SlotData = null;
                }
                
            }
            // 将物品拖拽到快捷栏
            else if (obj.tag.Equals("ShortcutSlot"))
            {
                /*ShortCutButtonController target = obj.GetComponent<ShortCutButtonController>();
                GDBase temp = target.ItemInGame;
                target.ItemInGame = ItemInGame;
                ItemInGame = temp;*/
            }
        }
        MesPlaneController.Instance.PointIconClose();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_slotData!=null)
        {
            MesPlaneController.Instance.PointIconShow(_slotData,GetComponent<RectTransform>().sizeDelta);
        }
    }

    private void ItemShow()
    {
        Icon.sprite = _slotData.Icon;
        Icon.enabled = true;
        if (_slotData.StackCount > 1)
        {
            num.text = _slotData.StackCount.ToString();
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
        if (_slotData != null)
        {
            // 根据不同物品类型调用使用方法
            switch (ItemUtil.GetItemType(_slotData.ID))
            {
                case 2:
                    // 数量大于0
                    if (_slotData.StackCount > 0)
                    {
                        // 尝试使用，需要传入使用目标。由于
                        if (_slotData.Use(_parentController.GdChaPlayer))
                        {
                            _slotData.StackCount -= 1;
                            if (_slotData.StackCount <= 0)
                            {
                                _slotData = null;
                                ItemUnShow();
                            }
                            else
                            {
                                num.text = _slotData.StackCount.ToString();
                            }
                        }
                    }
                    break;
            }
            MesPlaneController.Instance.CloseItemMes();
        }
    }
}
