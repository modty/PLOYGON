using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("性别选择")] 
    public GameObject female;

    public GameObject male;

    [Header("通用部分")] 
    public GameObject hair;
    public GameObject elfEar;
    public GameObject shoulderRight;
    public GameObject shoulderLeft;


    #region 身体部分

    private GameObject head;
    private GameObject eyebrows;
    private GameObject facialHair;

    #endregion
    #region 当前激活的物体

    private GameObject activeObject;

    #endregion

    private bool toggleOpen;


    public List<GameObject> hairsList,elfEarsList,headsList,eyebrowsList,facialHairsList;
    private void Awake()
    {
        SwitchGender(true);
    }

    private void InitCharacterParts()
    {
        head = activeObject.transform.GetChild(0).GetChild(0).gameObject;
        eyebrows = activeObject.transform.GetChild(1).gameObject;
        facialHair = activeObject.transform.GetChild(2).gameObject;

        hairsList = new List<GameObject>();
        headsList = new List<GameObject>();
        elfEarsList = new List<GameObject>();
        eyebrowsList = new List<GameObject>();
        facialHairsList = new List<GameObject>();
        
        GetAllChild(head,headsList,true);
        GetAllChild(elfEar,elfEarsList,false);
        GetAllChild(eyebrows,eyebrowsList,true);
        GetAllChild(facialHair,facialHairsList,false);
        GetAllChild(hair,hairsList,false);
    }


    public void GetAllChild(GameObject target, List<GameObject> targetList,bool baseActive)
    {
        for (int i = 0; i < target.transform.childCount; i++)
        {
            targetList.Add(target.transform.GetChild(i).gameObject);
            if (baseActive)
            {
                targetList[i].gameObject.SetActive(true);
                if (i > 0)
                {
                    targetList[i].SetActive(false);
                }
            }
            else
            {
                targetList[i].SetActive(false);
            }
        }
    }
    public void SwitchGender(bool toggle)
    {
        toggleOpen = toggle;
        female.SetActive(toggleOpen);
        male.SetActive(!toggleOpen);

        activeObject = toggle ? female : male;
        
        InitCharacterParts();
    }
}
