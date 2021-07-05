using Domain.Data.GameData;
using Domain.MessageEntities;

namespace Domain.Data.ActionData
{
    /// <summary>
    /// 物品使用
    /// </summary>
    public class ADUse
    {
        /// <summary>
        /// 物品使用者
        /// </summary>
        private GDBase _user;
        public GDBase User
        {
            get => _user;
            set => _user = value;
        }

        /// <summary>
        /// 使用物品
        /// </summary>
        private GDBase _item;
        public GDBase Item
        {
            get => _item;
            set => _item = value;
        }

        /// <summary>
        /// 使用目标
        /// </summary>
        private GDBase _target;
        public GDBase Target
        {
            get => _target;
            set => _target = value;
        }
        
        /// <summary>
        /// 使用，必须确定使用者以及使用物品。
        /// </summary>
        public bool Use()
        {
            if(_user==null||_item==null) return false;
            return DoUse();
        }
        /// <summary>
        /// 使用物品，必须确定使用者。
        /// </summary>
        public bool Use(GDBase item)
        {
            if(_target==null) return false;
            _item = item;
            return DoUse();
        }

        private MCombatTextCreate _mCombatTextCreate;
        
        private bool DoUse()
        {
            if (_item.CanUse() && _target.CanReceiveUse(_item))
            {
                return _item.Use(_target);
            }
            else
            {
                return false;
            }
        }
        
        public ADUse(GDBase user)
        {
            _user = user;
            _mCombatTextCreate = new MCombatTextCreate(this);
        }
    }
}