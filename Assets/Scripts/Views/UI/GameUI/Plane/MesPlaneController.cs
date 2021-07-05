using Domain.Data.GameData;
using UnityEngine;
using UnityEngine.UI;

public class MesPlaneController:MonoBehaviour
{
    private static MesPlaneController instance;
    public static MesPlaneController Instance => instance;
    [SerializeField] private GameObject itemMesPlane;
    [SerializeField] private Text itemMes;
    private RectTransform itemMesPlaneRectTransform;
    
    [SerializeField] private GameObject pointIconPlane;

    public GameObject PointIconPlane
    {
        get => pointIconPlane;
        set => pointIconPlane = value;
    }

    [SerializeField] private Image pointIcon;
    private RectTransform pointIconPlaneRectTransform;

    private GDBase _showData;

    public GDBase ShowData
    {
        get => _showData;
        set => _showData = value;
    }

    private void Awake()
    {
        instance = this;
        itemMesPlaneRectTransform = itemMesPlane.GetComponent<RectTransform>();
        pointIconPlaneRectTransform = pointIcon.gameObject.GetComponent<RectTransform>();
    }
    public void CalculatePointIconPlanePosition()
    {
        pointIconPlaneRectTransform.position = Input.mousePosition + new Vector3(-5, 5);
    }
    public void ShowItemMes(string text)
    {
        itemMes.text = text;
        CalculateMesPlanePosition();
        itemMesPlane.SetActive(true);
    }

    public void CloseItemMes()
    {
        itemMesPlane.SetActive(false);
    }
    public void CalculateMesPlanePosition()
    {
        float width = 1920;
        float height = 1080;
        Vector2 mousePosition = Input.mousePosition;
        // 设置锚点
        // 右半屏幕
        if (mousePosition.x > width / 2) itemMesPlaneRectTransform.pivot=new Vector2(1,1);
        // 左半屏幕
        else itemMesPlaneRectTransform.pivot=new Vector2(0,1);
        // 下面超出界面
        if (mousePosition.y - itemMesPlaneRectTransform.rect.height<0)
        {
            mousePosition.y+=itemMesPlaneRectTransform.rect.height - mousePosition.y;
        }
        itemMesPlaneRectTransform.position = mousePosition;
    }

    public void PointIconShow(GDBase gdBase,Vector2 size)
    {
        pointIconPlaneRectTransform.sizeDelta=size;
        pointIcon.sprite = gdBase.Icon;
        _showData = gdBase;
        pointIconPlane.SetActive(true);
    }

    public void PointIconClose()
    {
        pointIconPlane.SetActive(false);
    }
}
