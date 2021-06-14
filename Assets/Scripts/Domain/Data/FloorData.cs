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
    }
}