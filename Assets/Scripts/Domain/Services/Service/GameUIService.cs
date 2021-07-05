using Commons;
using Domain.MessageEntities;
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
        private GDChaPlayer _gdChaPlayer;
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
                GDChaPlayer gdChaPlayer=gameData.GameData as GDChaPlayer;
                if (gdChaPlayer != null)
                {
                    if (_gdChaPlayer != null && _gdChaPlayer.Uid == gdChaPlayer.Uid) return;
                    _gdChaPlayer = gdChaPlayer;
                    _bagBar.SetPlayer(_gdChaPlayer);
                    _inventory.SetPlayer(_gdChaPlayer);
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