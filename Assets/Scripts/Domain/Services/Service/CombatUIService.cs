using Loxodon.Framework.Services;
using Managers;
using UnityEngine;

namespace Domain.Services.IService
{
    /// <summary>
    /// 战斗UI服务，包括伤害显示
    /// </summary>
    public class CombatUIService:BaseService
    {
        private CombatUIController _combatUIController;

        public CombatUIService(IServiceContainer container):base(container)
        {
            _combatUIController = CombatUIController.Instance;
        }

        protected override void OnStart(IServiceContainer container)
        {
            _combatUIController.enabled = true;

        }

        protected override void OnStop(IServiceContainer container)
        {
            _combatUIController.enabled = false;

        }
    }
}