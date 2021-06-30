using Attribute.Items;
using Data;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// 地板
    /// </summary>
    public class FloorAttribute:GameData
    {
        public FloorAttribute(GameObject gameObject)
        {
            CanMoved = true;
            Transform = gameObject.transform;
        }

        protected override bool CanUse()
        {
            return false;
        }

        protected override void DoUse()
        {
            
        }

        public override bool CanReceiveUse(DataBase iData)
        {
            return false;
        }
    }
}