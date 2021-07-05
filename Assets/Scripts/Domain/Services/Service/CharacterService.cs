using System.Collections.Generic;
using Commons;
using Commons.Constants;
using Domain.Contexts;
using Domain.Data.GameData;
using Domain.MessageEntities;
using Loxodon.Framework.Services;
using Managers;
using Scripts;
using States;
using Tools;
using UnityEngine;
using UnityTemplateProjects.Domain.Params;
using AnimationState = States.AnimationState;

namespace Domain.Services.IService
{
    public class CharacterService:BaseService
    {
        /// <summary>
        /// 角色服务
        /// </summary>
        private Dictionary<string, CharacterContext> _characters;

        public CharacterService(IServiceContainer container):base(container)
        {
            _characters = new Dictionary<string, CharacterContext>();
            _mGameData = new MGameData(this,null);
        }

        #region 发布引用

        private MGameData _mGameData;
        

        #endregion
        public void CreateNewCharacter(CharacterParams cParams)
        {
            GameObject model = PrefabsManager.Instance.Character;
            model.transform.position = cParams.Position;
            GDChaPlayer data=new GDChaPlayer(model);
            DataController dc=model.GetComponent<DataController>();
            dc.GdCharacter = data;
            data.Uid = UidTool.Instance.RegistUid().ToString();
            data.RotateSpeed = cParams.RotateSpeed;
            data.AttackRange =cParams.AttackRange;
            data.AttackRange =cParams.AttackRange;
            data.MoveSpeed =cParams.MoveSpeed;
            data.MoveAcceleration = cParams.MoveAcceleration;
            data.WeaponType = cParams.WeaponType;
            data.TypedInteract = cParams.InteractType;
            
            data.AddAttribute(TypedAttribute.Health,new AHealth(cParams.Health));
            data.AddAttribute(TypedAttribute.Mana,new AMagic(cParams.Mana));
            data.AddAttribute(TypedAttribute.AttackDamage,new AAttackDamage(cParams.AttackDamage));
            data.AddAttribute(TypedAttribute.AttackSpeed,new AAttackSpeed(cParams.AttackSpeed));
            model.GetComponentInChildren<AnimEventController>().gdChaPlayer = data;
            CharacterContext characterContext = new CharacterContext("Character:" + data.Uid);
            GameObject o=CreateCharacterGameObject(data, true);
            characterContext.Set(Constants_Context.PlayerData, data);
            _characters.Add(data.Uid,characterContext);
            if (cParams.IsPlayer)
            {

                GDBase[] bagsEquipment = new GDBase[5];
                GDEquBackpack equBackpack = new GDEquBackpack().Assemble(1009000000);
                GDEquBackpack equBackpack1 = new GDEquBackpack().Assemble(1009000000);
                equBackpack.ContainData[0] = new GDConPotion().Assemble(2000000000000);
                equBackpack1.ContainData[0] = new GDConPotion().Assemble(2000000000001,10);
                bagsEquipment[0] = equBackpack;
                bagsEquipment[1] = equBackpack1;

                data.BagBarShortCutItems = bagsEquipment;

                data.BagOpenIndex = 0;
                _mGameData.GameData = data;
                _messenger.Publish(Constants_Event.ControlledCharacter,_mGameData);
                
                //Context.GetApplicationContext().GetContainer().Resolve<InputService>().SetControlledCharacter(data.Uid);
                
                
                // 注册角色更新调用
                List<BaseState> dcUpdateList = dc.UpdateList;
                List<BaseState> dcFixUpdate = dc.FixedUpdateList;
                dcUpdateList.Add(new AnimationState(data));
                dcUpdateList.Add(new SceneUIState());
                dcUpdateList.Add(new SkillAreaState(data));

                dcFixUpdate.Add(new MovementState(data));
                dcFixUpdate.Add(new NormalAttackState(data));
            }
        }
        
        private GameObject CreateCharacterGameObject(GDChaPlayer data,bool hasArea)
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

        private void LoadTestCharacters()
        {
            CharacterParams characterParams = new CharacterParams();
            characterParams.IsPlayer = true;
            characterParams.RotateSpeed = 3600f;
            characterParams.AttackRange = 1.2f;
            characterParams.MoveAcceleration = 12;
            characterParams.MoveSpeed = 5f;
            characterParams.WeaponType = TypedWeapon.Unarmed;
            characterParams.InteractType = TypedInteract.None;
            characterParams.Health = 4000;
            characterParams.Mana = 4000;
            characterParams.AttackDamage = 300;
            characterParams.AttackSpeed = 1.5f;
            characterParams.Position = Vector3.zero;
            CreateNewCharacter(characterParams);
            
            
            characterParams.IsPlayer = false;
            characterParams.RotateSpeed = 3600f;
            characterParams.AttackRange = 1.2f;
            characterParams.MoveAcceleration = 12;
            characterParams.WeaponType = TypedWeapon.Unarmed;
            characterParams.InteractType = TypedInteract.Enemy;
            characterParams.Health = 3000;
            characterParams.Mana = 4000;
            characterParams.AttackDamage = 200;
            characterParams.AttackSpeed = 1.5f;
            characterParams.Position = new Vector3(2, 0, 2);
            CreateNewCharacter(characterParams);
            
            characterParams.IsPlayer = false;
            characterParams.RotateSpeed = 3600f;
            characterParams.AttackRange = 1.2f;
            characterParams.MoveAcceleration = 12;
            characterParams.WeaponType = TypedWeapon.Unarmed;
            characterParams.InteractType = TypedInteract.Neutral;
            characterParams.Health = 2000;
            characterParams.Mana = 4000;
            characterParams.AttackDamage = 100;
            characterParams.AttackSpeed = 1.5f;
            characterParams.Position = new Vector3(0, 0, 2);
            CreateNewCharacter(characterParams);
            
            characterParams.IsPlayer = false;
            characterParams.RotateSpeed = 3600f;
            characterParams.AttackRange = 1.2f;
            characterParams.MoveAcceleration = 12;
            characterParams.WeaponType = TypedWeapon.Unarmed;
            characterParams.InteractType = TypedInteract.Ally;
            characterParams.Health = 1000;
            characterParams.Mana = 4000;
            characterParams.AttackDamage = 100;
            characterParams.AttackSpeed = 1.5f;
            characterParams.Position = new Vector3(-2, 0, 2);
            CreateNewCharacter(characterParams);
        }

        protected override void OnStart(IServiceContainer container)
        {
            LoadTestCharacters();
        }

        protected override void OnStop(IServiceContainer container)
        {
        }
    }
}