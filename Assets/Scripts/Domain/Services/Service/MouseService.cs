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
        public MouseService()
        {
            _mouseController = MouseController.Instance;
        }

        public new void Start()
        {
            _mouseController.enabled = true;
        }

        public new void Stop()
        {
            _mouseController.enabled = false;
        }
    }
}