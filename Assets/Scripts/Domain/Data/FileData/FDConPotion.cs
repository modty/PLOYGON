using System;
using System.Collections.Generic;
using Commons;
using Domain.Data.GameData;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Data.FileData
{
    [Serializable]
    public enum TypedPotionUse
    {
        UpdateAttribute
    }
    /// <summary>
    /// 消耗品：药水
    /// 定义：液态消耗品，必须通过容器盛放，可供制作。
    /// 用途：
    ///   一、改变基本属性
    ///     1、恢复类：使用后缓慢恢复（如：生命值、灵力值）,
    ///     2、持久类：使用后增加上限（如：增加生命上限、增加灵力上限）
    ///   二、造成伤害
    ///     1、投掷出去照成伤害：如火焰瓶、毒素瓶
    /// 制作条件：
    ///   一、物品条件
    ///     1、药水主材料（植物、动物等）
    ///     2、液体（稀释材料，水、蜂蜜等）
    ///     3、容器（玻璃容器等）。
    ///   二、技能条件
    ///     1、相关材料精通
    ///     2、制药精通
    ///   三、外部条件
    ///     1、药剂桌（可以不需要）
    /// </summary>
    [Serializable]
    public class FDConPotion:FDConsumable
    {
        /// <summary>
        /// 腐烂时间。-1时代表不会腐烂，数值为0时将会消失（或转换为其他物品）。
        /// </summary>
        public int decay;

        /// <summary>
        /// 腐烂后变为的物品ID，不会腐烂的不变化。
        /// </summary>
        public long decayed;

        /// <summary>
        /// 使用类型
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TypedPotionUse typedPotionUse;
        
        /// <summary>
        /// 消耗品该表的属性，对应TypedPotionUse为UpdateAttribute
        /// key：TypedAttribute
        /// value：
        ///     key：
        ///         value（int）：立即改变数值
        ///         times（int）：持续次数（为0时，代表没有持续效果，恢复一次）
        ///         valuePerTime（int）：每次改变数值
        ///         duration（int）：每次触发间隔时间
        /// </summary>
        public Dictionary<TypedAttribute,EUpdateAttribute> attributes;

    }
}