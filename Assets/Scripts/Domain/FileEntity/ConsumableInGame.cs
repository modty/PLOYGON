using System;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Data;
using Domain.Data;
using Domain.FileEntity;
using Scripts;
using Scripts.Commons.Utils;
using UnityEngine;

/// <summary>
/// 用户获取该消耗品后存在游戏中的对象。这样就能对已经获取到的物品进行个性修改
/// </summary>
[Serializable]
public class ConsumableInGame:Consumable,Useable
{
    public ConsumableInGame(Consumable consumable) : base(ItemUtil.Clone<Consumable>(consumable))
    {
    }
    /// <summary>
    /// 按照消耗品类别进行相应使用操作
    ///     0: 数值型消耗品，使用后增加属性。
    /// </summary>
    public bool Use(PlayerData target)
    {
        bool success = false;
        switch (ItemUtil.GetUseType(uid))
        {
            case 0:
                success=UseConsumableInGame0(target);
                break;
        }
        return success;
    }
    /// <summary>
    /// 使用恢复型消耗品
    /// </summary>
    public bool UseConsumableInGame0(GameData target)
    {
        foreach (KeyValuePair<string,Dictionary<string,int>> attribute in attributes)
        {
            switch (attribute.Key)
            {
                case "ShengMing":
                    ItemUse(target,TypedAttribute.Health,attribute.Value);
                    break;
                case "JingLi":
                    ItemUse(target,TypedAttribute.Mana,attribute.Value);
                    break;
            }
        }
        return true;
    }

    public void ItemUse(GameData target,TypedAttribute attribute,Dictionary<string,int> values)
    {
        values.TryGetValue("value", out var value);
        values.TryGetValue("times", out var times);
        values.TryGetValue("valuePerTime", out var valuePerTime);
        values.TryGetValue("duration", out var duration);
        AttributeChangeData attributeChangeData = new AttributeChangeData
        {
            Target = target, TypedAttributeChange = TypedAttributeChange.Instant
        };
        // 有持续属性
        if (times > 0)
        {
            attributeChangeData.TypedAttributeChange = TypedAttributeChange.Continuous;
        }
        attributeChangeData.AddAttribute(new AttributeChangeEntity(attribute,value,valuePerTime,times,duration));
        attributeChangeData.Use();
    }

    public bool CanUse()
    {
        return true;
    }
}


