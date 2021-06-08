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

        private PlayerAttribute _player;
        public Image content;
        private float currentFill=-1;
        public GameObject HeadBar
        {
            get => _headBar;
            set => _headBar = value;
        }

        public PlayerAttribute CharacterAttribute
        {
            get => _player;
            set
            {
                _player = value;
            }
        }
        private void Update()
        {
            var position = _player.TargetMovePosition;
            _headBar.transform.position = new Vector3(position.x,_player.Height+2,position.z);
            currentFill=1;
            if(Math.Abs(content.fillAmount - currentFill) > .01f) content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * 0.6f);
        }
    }
}