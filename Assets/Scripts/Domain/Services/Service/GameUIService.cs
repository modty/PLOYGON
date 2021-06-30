using Commons;
using Domain.MessageEntities;
using Items;
using Loxodon.Framework.Messaging;
using Loxodon.Framework.Services;
using Scripts;
using UnityEngine;

namespace Domain.Services.IService
{
    public class GameUIService:BaseService
    {
        private InventoryController _inventory;
        private BagBarController _bagBar;
        private PlayerData _player;
        private PrefabService _prefabService;
        public GameUIService(IServiceContainer container) : base(container)
        {
            _inventory=InventoryController.Instance;
            _bagBar=BagBarController.Instance;
            _prefabService = container.Resolve<PrefabService>();
            RegistSubscibes();
        }
        
        #region 订阅引用

        private ISubscription<MGameData> OnControlledCharacter_Change;

        #endregion
        private void RegistSubscibes()
        {
            OnControlledCharacter_Change=_messenger.Subscribe<MGameData>(Constants_Event.ControlledCharacter, (gameData) =>
            {
                PlayerData playerData=gameData.GameData as PlayerData;
                if (playerData != null)
                {
                    if (_player != null && _player.Uid == playerData.Uid) return;
                    _player = playerData;
                    _bagBar.SetPlayer(_player);
                    _inventory.SetPlayer(_player);
                }
            });
           
        }
        protected override void OnStart(IServiceContainer container)
        {
            _inventory.SlotPrefab = _prefabService.Get("Prefabs/UI/GameUI/InventorySlot");
            _bagBar.BagBarSlotPrefab = _prefabService.Get("Prefabs/UI/GameUI/BagBarSlot");
        }

        protected override void OnStop(IServiceContainer container)
        {
            
        }
    }
}