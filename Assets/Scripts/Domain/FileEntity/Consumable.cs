using System;
using System.Collections.Generic;

namespace Domain.FileEntity
{
    [Serializable]
    public class Consumable:FileBase
    {
        public Dictionary<string,Dictionary<string,int>> attributes;
        public Consumable(Consumable consumable)
        {
            name_cn = consumable.name_cn;
            uid = consumable.uid;
            attributes = consumable.attributes;
            description_cn = consumable.description_cn;
            icon = consumable.icon;
            maxStackSize = consumable.maxStackSize;
            price = consumable.price;
            capacity = consumable.capacity;
        }
        public Consumable()
        {
        }
    }
}