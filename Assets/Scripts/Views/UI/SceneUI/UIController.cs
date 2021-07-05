using System;
using System.Collections.Generic;
using ActionPool;
using Managers;
using UnityEngine;

namespace Scripts.Controller
{
    public class UIController:MonoBehaviour
    {
        private Dictionary<string, HeadBarController> _headBars;
        [SerializeField] private GameObject _headBarParent;
        private void Awake()
        {
            _headBars = new Dictionary<string, HeadBarController>();
        }
        /// <summary>
        /// 目标UI
        /// </summary>
        public GameObject TargetUI;
        
        public void AddHeadBar(GDChaPlayer character)
        {
            GameObject headbar = PrefabsManager.Instance.HeadBar;
            HeadBarController barController = headbar.GetComponent<HeadBarController>();
            barController.Character = character;
            barController.HeadBar = headbar;
            headbar.name = character.Uid.ToString();
            headbar.transform.SetParent(_headBarParent.transform);
            _headBars[character.Uid] = barController;
        }

        public void RemoveHeadBar(GDChaPlayer character)
        {
            Destroy(_headBars[character.Uid].gameObject);
            _headBars.Remove(character.Uid);
        }
    }
}