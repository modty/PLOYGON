using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftUIController: MonoBehaviour
{
    [SerializeField] private GameObject buttonCraft;
    [SerializeField] private GameObject craftSub;
    [SerializeField] private GameObject craftInfo;
    [SerializeField] private GameObject craft;

    private CraftPanelController _craftPanelController;
    // Start is called before the first frame update
    void Start()
    {
        _craftPanelController = craft.GetComponent<CraftPanelController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowCraft(bool instant = false)
    {
        _craftPanelController.Toggle();
    }
}
