using System;
using System.Collections.Generic;
using ActionPool;
using Managers;
using UnityEngine;

namespace Scripts.Controller
{
    public class UIController:MonoBehaviour
    {
        private Dictionary<long, HeadBarController> _headBars;
        [SerializeField] private GameObject _headBarParent;
        private void Awake()
        {
            _headBars = new Dictionary<long, HeadBarController>();
        }
        /// <summary>
        /// 目标UI
        /// </summary>
        public GameObject TargetUI;
        
        public void AddHeadBar(PlayerAttribute characterAttribute)
        {
            GameObject headbar = PrefabsManager.Instance.HeadBar;
            HeadBarController barController = headbar.GetComponent<HeadBarController>();
            barController.CharacterAttribute = characterAttribute;
            barController.HeadBar = headbar;
            headbar.name = characterAttribute.Uid.ToString();
            headbar.transform.SetParent(_headBarParent.transform);
            _headBars[characterAttribute.Uid] = barController;
        }

        public void RemoveHeadBar(PlayerAttribute characterAttribute)
        {
            Destroy(_headBars[characterAttribute.Uid].gameObject);
            _headBars.Remove(characterAttribute.Uid);
        }
    }
}