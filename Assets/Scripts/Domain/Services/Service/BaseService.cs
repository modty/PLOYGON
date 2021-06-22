using Loxodon.Framework.Messaging;

namespace Domain.Services.IService
{
    public class BaseService:IBaseService
    {
        protected Messenger _messenger;
        protected BaseService()
        {
            _messenger=Messenger.Default;
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}