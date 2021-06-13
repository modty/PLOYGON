using Data;
using Loxodon.Framework.Messaging;

namespace Domain.MessageEntities
{
    public class MGameData:MessageBase
    {
        private GameData _gameData;
        public MGameData(object sender,GameData gameData) : base(sender)
        {
            _gameData = gameData;
        }

        public GameData GameData
        {
            get => _gameData;
            set => _gameData = value;
        }
    }
}