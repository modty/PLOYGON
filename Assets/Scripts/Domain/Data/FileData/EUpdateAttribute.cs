using System;

namespace Domain.Data.FileData
{
    [Serializable]
    public class EUpdateAttribute
    {
        public int value;
        public int valuePerTime;
        public int times;
        public int duration;
    }
}