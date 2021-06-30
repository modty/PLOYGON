
using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagBarButtonController : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [SerializeField] private Image icon;
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
        else ItemUnShow();
    }
    public void openCloseInventory()
    {
        InventoryController.Instance.OpenClose(ItemInGame);
    }
    public void OnDrag(PointerEventData eventData)
    {
        MesPlaneController.Instance.CalculatePointIconPlanePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if(obj.tag.Equals("BagBarSlot"))
        {
            BagBarButtonController target = obj.GetComponent<BagBarButtonController>();
            ItemInGame temp = target.ItemInGame;
            target.ItemInGame = ItemInGame;
            ItemInGame = temp;
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
    }

    private void ItemUnShow()
    {
        Icon.enabled = false;
    }
}

