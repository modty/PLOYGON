using System;
using System.Collections.Generic;
using Scripts;
using Scripts.Commons.Utils;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class EquipmentInGame : Equipment
{
    public EquipmentInGame(Equipment equipment) : base(
        ItemUtil.Clone<Equipment>(equipment))
    {
    }
    /// <summary>
    /// 装备装备
    ///     6: 双手武器
    /// </summary>
    public bool Use()
    {
        bool success = false;
        switch (ItemUtil.GetUseType(uid))
        {
            case 6:
                success=UseEquipmentInGame();
                break;
        }
        return success;
    }

    public bool UseEquipmentInGame()
    {
        /*CharacterAttribute characterAttribute = CharactersManager.Instance.Get(BelongTo);

        switch (Utils.GetUseType(uid))
        {
            case 6:
                Random random = new Random();
                characterAttribute.EquipEquipment(hands[random.Next(0,hands.Length)],this);
                break;
        }*/
        return true;
    }
}
