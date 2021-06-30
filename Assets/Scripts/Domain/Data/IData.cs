using Attribute.Items;
using Loxodon.Framework.Messaging;

namespace Domain.Data
{
    public abstract class IData
    {
        protected Messenger _messenger;

        protected IData()
        {
            _messenger=Messenger.Default;
        }

        protected abstract bool CanUse();
        public void Use()
        {
            if (CanUse())
            {
                DoUse();
            }
        }
        protected abstract void DoUse();
        public abstract bool CanReceiveUse(DataBase iData);
    }
}