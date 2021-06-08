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

        public void CreateText(Vector3 position, string text, SCTTYPE type, bool crit)
        {
            //Offset
            position.z += 0.8f;
            Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
            sct.transform.position = position;
            string before = string.Empty;
            string after = string.Empty;
            switch (type)
            {
                case SCTTYPE.DAMAGE:
                    sct.color = Color.red;
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


    }

}