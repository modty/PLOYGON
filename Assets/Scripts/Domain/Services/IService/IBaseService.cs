using Loxodon.Framework.Services;

namespace Domain.Services.IService
{
    public interface IBaseService
    {
        void Start();
        void Stop();
    }
}