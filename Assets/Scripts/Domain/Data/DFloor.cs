using Attribute.Items;
using Data;
using Domain.Data.GameData;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// 地板
    /// </summary>
    public class DFloor:GDCharacter
    {
        public DFloor(GameObject gameObject)
        {
            CanMoved = true;
            Transform = gameObject.transform;
        }

        public override bool CanUse()
        {
            return false;
        }

        public override bool Use(GDBase target)
        {
            return false;
        }

        public override bool CanReceiveUse(GDBase data)
        {
            return false;
        }
    }
}