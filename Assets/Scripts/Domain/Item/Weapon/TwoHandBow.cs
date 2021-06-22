using Data;
using UnityEngine;

namespace Domain.Item.Weapon
{
    [CreateAssetMenu(fileName = "GameData", menuName = "新增物品/新增装备/新增武器/双手弓", order = 0)]
    public class TwoHandBow : ScriptableObject
    {
        public GameData gameData;
        [Header("攻击计时")]
        public bool override_timing = false; //设置为True时，默认攻击前后摇将被覆盖
        public float attack_windup = 0.7f;
        public float attack_windout = 0.4f;

        public float attack_damage;
        public float attack_range;
        
    }
}