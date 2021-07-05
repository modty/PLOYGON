using Commons;
using Data;
using UnityEngine;

namespace Scripts
{
    public class DataFactory
    {
        public  static GDCharacter CreateAssetMenuAttribute(TypedGameData gameData,GameObject gameObject)
        {
            GDCharacter gdCharacter = null;
            switch (gameData)
            {
                case TypedGameData.Player:
                case TypedGameData.Character:
                    gdCharacter= new GDChaPlayer(gameObject);
                    break;
                case TypedGameData.Floor:
                    gdCharacter= new DFloor(gameObject);
                    break;
            }

            return gdCharacter;
        }
    }
}