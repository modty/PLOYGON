using System;
using Scripts.Commons;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Scripts.Controller
{
    public class HeadBarController:MonoBehaviour
    {

        private GameObject _headBar;

        private GDChaPlayer _gdChaPlayer;
        public Image content;
        private float currentFill=-1;
        public GameObject HeadBar
        {
            get => _headBar;
            set => _headBar = value;
        }

        public GDChaPlayer Character
        {
            get => _gdChaPlayer;
            set
            {
                _gdChaPlayer = value;
            }
        }
        private void Update()
        {
            var position = _gdChaPlayer.TargetMovePosition;
            _headBar.transform.position = new Vector3(position.x,_gdChaPlayer.Height+2,position.z);
            currentFill=1;
            if(Math.Abs(content.fillAmount - currentFill) > .01f) content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * 0.6f);
        }
    }
}