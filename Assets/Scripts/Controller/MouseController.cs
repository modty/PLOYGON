using System;
using ActionPool;
using Commons;
using UnityEngine;
using GameData = Data.GameData;

namespace Scripts
{
/// <summary>
    /// 鼠标图片类型
    /// </summary>
    public enum CursorType
    {
        Normal=0, // 普通图标
        InteractAlly=1, //友方互动
        InteractEnemy=2, //敌方互动
        AttackAlly=3, //攻击
        AttackEnemy=4, //攻击
        ExtraNormal=5, //攻击
        ExtraAttack=6, //攻击
    }

    /// <summary>
    /// 改变鼠标的贴图
    /// </summary>
    public class MouseController: MonoBehaviour
    {
        private CursorType _currentType;
        [SerializeField]private Texture2D normal;
        [SerializeField]private Texture2D interactAlly;
        [SerializeField]private Texture2D interactEnemy;
        [SerializeField]private Texture2D attackAlly;
        [SerializeField]private Texture2D attackEnemy;
        [SerializeField]private Texture2D extraNormal;
        [SerializeField]private Texture2D extraAttack;
        private static MouseController _instance;
        private TypedInteract _typedInteract;
        private GameData _gameData;
        private RaycastHit hitInfo;
        private Vector3 _mousePosition;
        private bool _preFroceAttack;
        private bool _forceAttack;
        public GameData GameData
        {
            get => _gameData;
            set => _gameData = value;
        }

        public Vector3 MousePosition
        {
            get => _mousePosition;
            set => _mousePosition = value;
        }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            
            Cursor.SetCursor(normal,Vector2.zero, CursorMode.Auto);
            RegistInputActions();
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo))
            {
                _gameData= hitInfo.transform.GetComponent<DataController>().GameData;
                OnMouseOverSomething(_gameData);
                _mousePosition = hitInfo.point;
            }
            
        }
        
        
        private void OnMouseOverSomething(GameData gameData)
        {
            if(gameData==null||(_typedInteract.Equals(gameData.TypedInteract)&&_preFroceAttack==_forceAttack)) return;
            _typedInteract = gameData.TypedInteract;
            switch (_typedInteract)
            {
                case TypedInteract.None:
                case TypedInteract.Neutral:
                    Cursor.SetCursor(normal,Vector2.zero, CursorMode.Auto);
                    break;
                case TypedInteract.Ally:
                    Cursor.SetCursor(_forceAttack?attackAlly:interactAlly,Vector2.zero, CursorMode.Auto);
                    break;
                case TypedInteract.Enemy:
                    Cursor.SetCursor(_forceAttack?attackEnemy:interactEnemy,Vector2.zero, CursorMode.Auto);
                    break;
            }

            _preFroceAttack = _forceAttack;
        }

        public static MouseController Get()
        {
            return _instance;
        }

        
        public void CursorChange(CursorType cursorType)
        {
            if(_currentType==cursorType) return;
            _currentType = cursorType;
            switch (cursorType)
            {
                case CursorType.Normal:
                    Cursor.SetCursor(normal,Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.InteractAlly:
                    Cursor.SetCursor(interactAlly,Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.InteractEnemy:
                    Cursor.SetCursor(interactEnemy,Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.AttackAlly:
                    Cursor.SetCursor(attackAlly,Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.AttackEnemy:
                    Cursor.SetCursor(attackEnemy,Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.ExtraNormal:
                    Cursor.SetCursor(extraNormal,Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.ExtraAttack:
                    Cursor.SetCursor(extraAttack,Vector2.zero, CursorMode.Auto);
                    break;
            }
        }
        
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            EventCenter.AddListener(TypedInputActions.OnForceAttack.ToString(),OnForceAttack);
            EventCenter.AddListener(TypedInputActions.OffForceAttack.ToString(),OffForceAttack);
        }

        private void OnForceAttack()
        {
            _forceAttack = true;
        }
        private void OffForceAttack()
        {
            _forceAttack = false;
        }
    }
}