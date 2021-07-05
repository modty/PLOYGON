using Data;
using Domain.Data.GameData;
using Loxodon.Framework.Messaging;

namespace Domain.MessageEntities
{
    public class MGameData:MessageBase
    {
        private GDCharacter _gameData;
        public MGameData(object sender,GDCharacter gameData) : base(sender)
        {
            _gameData = gameData;
        }

        public GDCharacter GameData
        {
            get => _gameData;
            set => _gameData = value;
        }
    }
}