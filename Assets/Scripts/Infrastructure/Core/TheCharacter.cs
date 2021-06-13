using System.Collections.Generic;
using Commons;
using Scripts;
using Tools;
using UnityEngine;

namespace Managers
{
    public class TheCharacter
    {
        private static TheCharacter _instance;

        public static TheCharacter Instance
        {
            get
            {
                if (_instance == null) _instance = new TheCharacter();
                return _instance;
            }
        }
        private Dictionary<long, PlayerAttribute> _characters;
        public TheCharacter()
        {
            _characters = new Dictionary<long, PlayerAttribute>();
            LoadCharacterFromDisk();
        }
        
        /// <summary>
        /// 从存档文件中加载角色
        /// </summary>
        private void LoadCharacterFromDisk()
        {
            GameObject model = PrefabsManager.Instance.Character;

            PlayerAttribute attribute=new PlayerAttribute(model);
            attribute.Uid = UidTool.Instance.RegistUid();
            _characters.Add(attribute.Uid,attribute);
            attribute.RotateSpeed = 3600f;
            attribute.AttackRange = 1.2f;
            attribute.NormalAttackAnimSpeed = 1;
            attribute.MoveAcceleration = 12;
            attribute.WeaponType = TypedWeapon.Unarmed;
            attribute.Health = new Health(4335);
            attribute.AttackDamage = new AttackDamage(234);
            attribute.AttackSpeed = new AttackSpeed(5);
            model.GetComponentInChildren<AnimEventController>()._player = attribute;
            GameObject o=CreateCharacterGameObject(attribute, true);
            InputController controller =o.AddComponent<InputController>();
            controller._attribute = attribute;
        }

        private GameObject CreateCharacterGameObject(PlayerAttribute attribute,bool hasArea)
        {
            GameObject o = new GameObject
            {
                name = "Character_" + attribute.Uid,
                transform = { parent = GameObjectManager.Instance.CharactersParent.transform}
            };
            attribute.Transform.parent = o.transform;
            attribute.Transform.name = o.name + "_Model";
            // 是否含有范围指示器
            if (hasArea)
            {
                GameObject skillArea = PrefabsManager.Instance.SkillArea;
                skillArea.name = o.name + "_SkillArea";
                skillArea.transform.parent = o.transform;
            
                attribute.AttackRangeUI = skillArea.transform;
            }
            return o;
        }
    }
}