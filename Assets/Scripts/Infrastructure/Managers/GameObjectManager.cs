using UnityEngine;

namespace Managers
{
    public class GameObjectManager:MonoBehaviour
    {
        private static GameObjectManager _instance;

        public static GameObjectManager Instance => _instance;
        
        private void Awake()
        {
            _instance = this;
        }

        [SerializeField] private GameObject charactersParent;

        public GameObject CharactersParent => charactersParent;
    }
}