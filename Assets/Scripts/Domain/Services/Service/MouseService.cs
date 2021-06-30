using Loxodon.Framework.Services;
using Scripts;

namespace Domain.Services.IService
{
    public enum CursorType
    {
        Normal=0, // 普通图标
        InteractAlly=1, //友方互动
        InteractEnemy=2, //敌方互动
        AttackAlly=3, //攻击
        AttackEnemy=4, //攻击
        ExtraNormal=5, //攻击
        ExtraAttack=6, //攻击
    }
    /// <summary>
    /// 鼠标服务
    /// </summary>
    public class MouseService:BaseService
    {
        private MouseController _mouseController;        
        public MouseService(IServiceContainer container):base(container)
        {
            _mouseController = MouseController.Instance;
        }


        protected override void OnStart(IServiceContainer container)
        {
            _mouseController.enabled = true;

        }


        protected override void OnStop(IServiceContainer container)
        {
            _mouseController.enabled = false;
        }
    }
}