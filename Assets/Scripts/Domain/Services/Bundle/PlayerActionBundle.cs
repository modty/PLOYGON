using Loxodon.Framework.Services;

namespace Domain.Service
{
    /// <summary>
    /// 用户行为服务（用户行为服务注册到全局上下文中，所有与用户相关的操作、输入输出、都通过此进行管理）
    /// 
    /// </summary>
    public class PlayerActionBundle: AbstractServiceBundle
    {
        public PlayerActionBundle(IServiceContainer container) : base(container)
        {
        }

        protected override void OnStart(IServiceContainer container)
        {
        }

        protected override void OnStop(IServiceContainer container)
        {
            
        }
    }
}