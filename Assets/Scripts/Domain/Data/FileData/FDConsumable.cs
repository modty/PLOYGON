using System;
using System.Collections.Generic;
using Commons;

namespace Domain.Data.FileData
{
    /// <summary>
    /// (FileData中对象只会在游戏运行过程中存在一个。)
    /// (通过FileData对象初始化GameData对象)
    /// (FileData中所有属性均为Public)
    /// 【消耗品】
    /// 消耗品定义：使用一次后消失且该对象将不会再被引用。
    /// 如：药水、餐食、丹药、一次性技能等。
    /// </summary>
    [Serializable]
    public class FDConsumable:FDBase
    {
        
        
        
    }
}