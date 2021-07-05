using System.Collections.Generic;
using Commons;
using Data;
using Domain.Data.ActionData;
using Domain.Data.FileData;
using Scripts;
using Scripts.Commons.Utils;
using UnityEngine.Diagnostics;

namespace Domain.Data.GameData
{
    public class GDConPotion:GDConsumable
    {
        private FDConPotion _fdConPotion;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">药水可堆叠</param>
        /// <param name="count">初始化堆叠数量（不能超过最大堆叠数）</param>
        /// <returns></returns>
        public GDConPotion Assemble(long id,int count=1)
        {
            FDConPotion potion = _fdService.GetFD(id) as FDConPotion;
            if (potion == null) return null;
            _fdConPotion = ItemUtil.Clone(potion);
            _icon = _resourceService.LoadSpriteByIO(_fdConPotion.icon);
            _title = potion.title;
            _id = potion.id;
            if (potion.maxStackSize>0)
            {
                if (potion.maxStackSize >= count)
                {
                    // 默认数量为1
                    _stackCount = count;
                }
                else
                {
                    _stackCount = potion.maxStackSize;
                }
            }

            return this;
        }

        /// <summary>
        /// 药水能够被使用
        /// </summary>
        /// <returns></returns>
        public override bool CanUse()
        {
            return true;
        }

        public override bool Use(GDBase target)
        {
            bool success = false;
            switch (_fdConPotion.typedPotionUse)
            {
                case TypedPotionUse.UpdateAttribute:
                    success=UsePotionUpdateAttribute(target as GDCharacter);
                    break;
            }
            return success;
        }

        private bool UsePotionUpdateAttribute(GDCharacter target)
        {
            if (target == null) return false;
            foreach (KeyValuePair<TypedAttribute,EUpdateAttribute> pair in _fdConPotion.attributes)
            {
                ABaseAttribute attribute=target.GetAttribute<ABaseAttribute>(pair.Key);
                EUpdateAttribute data = pair.Value;
                if (data.value > 0) attribute.UpdateCurrentValue(data.value);
                if (data.times > 0)
                {
                    int times=0;
                    Timer _testTimer = null;
                    _testTimer = Timer.Register(
                        duration: data.duration/1000f,
                        () =>
                        {
                            attribute.UpdateCurrentValue(data.valuePerTime);
                            times++;
                            if (times >= data.times)
                            {
                                _testTimer.Cancel();
                            }
                        },
                        isLooped: true);
                }
            }
            return true;
        }
        /// <summary>
        /// 药水不能接收其他物品的使用
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool CanReceiveUse(GDBase data)
        {
            return false;
        }
    }
}