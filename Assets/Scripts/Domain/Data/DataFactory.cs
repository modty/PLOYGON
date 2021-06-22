﻿using Commons;
using UnityEngine;
using GameData = Data.GameData;

namespace Scripts
{
    public class DataFactory
    {
        public  static GameData CreateAssetMenuAttribute(TypedGameData gameData,GameObject gameObject)
        {
            GameData _gameData = null;
            switch (gameData)
            {
                case TypedGameData.Player:
                case TypedGameData.Character:
                    _gameData= new PlayerData(gameObject);
                    break;
                case TypedGameData.Floor:
                    _gameData= new FloorAttribute(gameObject);
                    break;
            }

            return _gameData;
        }
    }
}