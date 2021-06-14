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
        private Dictionary<long, PlayerData> _characters;
        public TheCharacter()
        {
            _characters = new Dictionary<long, PlayerData>();
            LoadCharacterFromDisk();
        }
        
        /// <summary>
        /// 从存档文件中加载角色
        /// </summary>
        private void LoadCharacterFromDisk()
        {
            GameObject model = PrefabsManager.Instance.Character;

            PlayerData data=new PlayerData(model);
            data.Uid = UidTool.Instance.RegistUid();
            _characters.Add(data.Uid,data);
            data.RotateSpeed = 3600f;
            data.AttackRange = 1.2f;
            data.NormalAttackAnimSpeed = 1;
            data.MoveAcceleration = 12;
            data.WeaponType = TypedWeapon.Unarmed;
            data.Health = new AHealth(4335);
            data.AttackDamage = new AAttackDamage(234);
            data.AttackSpeed = new AAttackSpeed(5);
            model.GetComponentInChildren<AnimEventController>()._player = data;
            GameObject o=CreateCharacterGameObject(data, true);
            InputController controller =o.AddComponent<InputController>();
            controller.data = data;
        }

        private GameObject CreateCharacterGameObject(PlayerData data,bool hasArea)
        {
            GameObject o = new GameObject
            {
                name = "Character_" + data.Uid,
                transform = { parent = GameObjectManager.Instance.CharactersParent.transform}
            };
            data.Transform.parent = o.transform;
            data.Transform.name = o.name + "_Model";
            // 是否含有范围指示器
            if (hasArea)
            {
                GameObject skillArea = PrefabsManager.Instance.SkillArea;
                skillArea.name = o.name + "_SkillArea";
                skillArea.transform.parent = o.transform;
            
                data.AttackRangeUI = skillArea.transform;
            }
            return o;
        }
    }
}