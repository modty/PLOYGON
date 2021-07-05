using Domain.Data;

namespace Attribute.Items
{
    public abstract class DataBase:IData
    {
        /// <summary>
        /// 名称
        /// </summary>
        protected string _title;
        /// <summary>
        /// 简介
        /// </summary>
        protected string _description;

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

    }
}