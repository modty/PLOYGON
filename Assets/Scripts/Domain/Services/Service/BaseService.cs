using Loxodon.Framework.Messaging;
using Loxodon.Framework.Services;

namespace Domain.Services.IService
{
    public abstract class BaseService:AbstractServiceBundle
    {
        protected Messenger _messenger;
        protected BaseService(IServiceContainer container):base(container)
        {
            _messenger=Messenger.Default;
        }

    }
}