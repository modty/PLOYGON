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

        public CombatUIService()
        {
            _combatUIController = CombatUIController.Instance;
        }

        public new void Start()
        {
            _combatUIController.enabled = true;
        }

        public new void Stop()
        {
            _combatUIController.enabled = false;
        }
    }
}