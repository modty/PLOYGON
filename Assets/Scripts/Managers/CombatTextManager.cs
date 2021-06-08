using UnityEngine;
using UnityEngine.UI;

namespace Managers
{

    public enum SCTTYPE {DAMAGE,HEAL,XP,MANA}

    public class CombatTextManager : MonoBehaviour
    {

        private static CombatTextManager instance;

        public static CombatTextManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CombatTextManager>();
                }
                return instance;
            }
        }

        [SerializeField]
        private GameObject combatTextPrefab;

        public void CreateText(Vector3 position, string text, SCTTYPE type, bool crit,bool direction)
        {
            //Offset
            GameObject gb = Instantiate(combatTextPrefab, transform);
            CombatText combatText = gb.GetComponent<CombatText>();
            combatText.Direction = direction ? 1 : -1;
            Text sct = gb.GetComponent<Text>();
            sct.transform.position = position;
            string before = string.Empty;
            string after = string.Empty;
            switch (type)
            {
                case SCTTYPE.DAMAGE:
                    sct.color = GetColor("#D0770B");
                    break;
                case SCTTYPE.HEAL:
                    before = "+";
                    sct.color = Color.green;
                    break;
                case SCTTYPE.XP:
                    before = "+";
                    after = " XP";
                    sct.color = Color.yellow;
                    break;
                case SCTTYPE.MANA:
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