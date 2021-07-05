
using Domain.Data.GameData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagBarButtonController : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    [SerializeField] private Image icon;

    private BagBarController _parentController;
    private int _index;

    public int Index
    {
        get => _index;
        set => _index = value;
    }
    public BagBarController ParentController
    {
        get => _parentController;
        set => _parentController = value;
    }

    public Image Icon => icon;
    private GDEquBackpack _backpack;

    public GDEquBackpack Backpack
    {
        get => _backpack;
        set => SwapItem(value);
    }
    
    private void SwapItem(GDEquBackpack fromItem)
    {
        _backpack = fromItem;
        if (fromItem != null) ItemShow();
        else ItemUnShow();
    }
    public void OpenCloseInventory()
    {
        InventoryController.Instance.OpenClose(_backpack);
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
            GDEquBackpack temp = target.Backpack;
            target.Backpack = _backpack;
            Backpack = temp;
        }
        MesPlaneController.Instance.PointIconClose();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_backpack!=null)
        {
            MesPlaneController.Instance.PointIconShow(_backpack,GetComponent<RectTransform>().sizeDelta);
        }
    }
    private void ItemShow()
    {
        Icon.sprite = _backpack.Icon;
        Icon.enabled = true;
    }

    private void ItemUnShow()
    {
        Icon.enabled = false;
    }
}

