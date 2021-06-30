using System;
using Commons;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{

    public enum SCTTYPE {Heal,HealDamage,XP,Mana,ManaDamage}

    /// <summary>
    /// 管理场景UI：战斗文本，如伤害数值等
    /// </summary>
    public class CombatUIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject combatTextPrefab;

        private static CombatUIController _instance;

        public static CombatUIController Instance
        {
            get => _instance;
        }

        private Messenger messenger;

        private void Awake()
        {
            _instance = this;
            messenger=Messenger.Default;
            RegistSubscribes();
            enabled = false;
        }


        #region 监听引用

        private ISubscription<MCombatTextCreate> onCombatTextCreate;
        

        #endregion
        /// <summary>
        /// 注册监听
        /// </summary>
        private void RegistSubscribes()
        {
            onCombatTextCreate = messenger.Subscribe<MCombatTextCreate>(Constants_Event.CombatTextCreate, (message) =>
            {
                CreateText(message.Position,message.Text,message.Type,message.Crit,message.Direction);
            });
        }

        public void CreateText(Vector3 position, string text, SCTTYPE type, bool crit,bool direction)
        {
            //Offset
            GameObject gb = Instantiate(combatTextPrefab, transform);
            CombatTextController combatTextController = gb.GetComponent<CombatTextController>();
            combatTextController.Direction = direction ? 1 : -1;
            Text sct = gb.GetComponent<Text>();
            sct.transform.position = position;
            string before = string.Empty;
            string after = string.Empty;
            switch (type)
            {
                case SCTTYPE.HealDamage:
                    sct.color = GetColor("#D0770B");
                    break;
                case SCTTYPE.Heal:
                    before = "+";
                    sct.color = Color.green;
                    break;
                case SCTTYPE.XP:
                    before = "+";
                    after = " XP";
                    sct.color = Color.yellow;
                    break;
                case SCTTYPE.Mana:
                    before = "+";
                    sct.color = Color.cyan;
                    break;
            }

            sct.text = before + text + after;

            if (crit)
            {
                sct.GetComponent<Animator>().SetBool("Crit", crit);
            }
        }
        public Color GetColor(string color)
        {
            if (color.Length == 0)
            {
                return Color.black;//设为黑色
            }
            else
            {
                //#ff8c3 除掉#
                color = color.Substring(1);
                int v = int.Parse(color, System.Globalization.NumberStyles.HexNumber);
                //转换颜色
                return new Color(
                    //int>>移位 去低位
                    //&按位与 去高位
                    ((float)(((v >> 16) & 255))) / 255,
                    ((float)((v >> 8) & 255)) / 255,
                    ((float)((v >> 0) & 255)) / 255
                );
            }
        }

    }

}