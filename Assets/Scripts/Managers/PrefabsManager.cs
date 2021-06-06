using UnityEngine;

namespace Managers
{
 public class PrefabsManager : MonoBehaviour
    {
        private static PrefabsManager _instance;

        public static PrefabsManager Instance => _instance;
        
        private void Awake()
        {
            _instance = this;
        }

        [SerializeField] private GameObject _clickArrow;
        [SerializeField] private Transform _clickArrowParent;
        [SerializeField] private GameObject _normalAttackArea;
        [SerializeField] private GameObject _character;
        [SerializeField] private GameObject _characterNoControll;
        [SerializeField] private GameObject _armor;
        [SerializeField] private GameObject _characterDefault;
        [SerializeField] private GameObject _equipmentModel;
        [SerializeField] private GameObject _headBar;
        [SerializeField] private Transform _effectParent;
        public GameObject HeadBar {
            get => Instantiate(_headBar);
        }
        public GameObject EquipmentModel{
            get => Instantiate(_equipmentModel);
        }
        public GameObject CharacterDefault{
            get => Instantiate(_characterDefault);
        }
        public GameObject Armor
        {
            get => Instantiate(_armor);
        }

        public GameObject CharacterNoControll
        {
            get
            {
                GameObject obj = Instantiate(_characterNoControll);
                return obj;
            }
        }

        public GameObject Character
        {
            get
            {
                GameObject obj = Instantiate(_character);
                return obj;
            }
        }

        public GameObject ClickArrow
        {
            get
            {
                GameObject obj = Instantiate(_clickArrow, _clickArrowParent, true);
                Destroy(obj, 1.1f);
                return obj;
            }
        }
        public GameObject NormalAttackArea
        {
            get
            {
                GameObject obj = Instantiate(_normalAttackArea);
                return obj;
            }
        }

        public GameObject LoadPrefab(string url,Transform transform)
        {
            return Instantiate(Resources.Load<GameObject>("Prefabs/"+url),transform);
        }
    }
}