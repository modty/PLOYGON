using Domain.Data;

namespace Attribute.Items
{
    public abstract class DataBase:IData
    {
        protected string _name;
        protected string _description;
        protected long _uid;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public long Uid
        {
            get => _uid;
            set => _uid = value;
        }

    }
}