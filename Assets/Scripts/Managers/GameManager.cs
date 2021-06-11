using System;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 游戏管理
    /// </summary>
    public class GameManager:MonoBehaviour
    {
        // 游戏中的所有角色
        private TheCharacter _character;
        private void Start()
        {
            _character = TheCharacter.Instance;
        }
    }
}