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
        
        public void AddHeadBar(PlayerData characterData)
        {
            GameObject headbar = PrefabsManager.Instance.HeadBar;
            HeadBarController barController = headbar.GetComponent<HeadBarController>();
            barController.CharacterData = characterData;
            barController.HeadBar = headbar;
            headbar.name = characterData.Uid.ToString();
            headbar.transform.SetParent(_headBarParent.transform);
            _headBars[characterData.Uid] = barController;
        }

        public void RemoveHeadBar(PlayerData characterData)
        {
            Destroy(_headBars[characterData.Uid].gameObject);
            _headBars.Remove(characterData.Uid);
        }
    }
}