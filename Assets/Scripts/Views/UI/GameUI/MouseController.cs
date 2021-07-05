using System.Globalization;
using ActionPool;
using Commons;
using Data;
using Domain.MessageEntities;
using Domain.Services.IService;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Localizations;
using Loxodon.Framework.Messaging;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// 改变鼠标的贴图
    /// </summary>
    public class MouseController: MonoBehaviour
    {
        private CursorType _currentType;
        private Messenger _messenger;
        [SerializeField]private Texture2D normal;
        [SerializeField]private Texture2D interactAlly;
        [SerializeField]private Texture2D interactEnemy;
        [SerializeField]private Texture2D attackAlly;
        [SerializeField]private Texture2D attackEnemy;
        [SerializeField]private Texture2D extraNormal;
        [SerializeField]private Texture2D extraAttack;
        private static MouseController _instance;

        public static MouseController Instance
        {
            get => _instance;
            set => _instance = value;
        }

        private TypedInteract _typedInteract;
        private GDCharacter _gdCharacter;
        private RaycastHit hitInfo;
        private Vector3 _mousePosition;
        private bool _preFroceAttack;
        private bool _forceAttack;
        public GDCharacter GdCharacter
        {
            get => _gdCharacter;
            set => _gdCharacter = value;
        }

        public Vector3 MousePosition
        {
            get => _mousePosition;
            set => _mousePosition = value;
        }

        private void Awake()
        {
            _instance = this;
            _messenger=Messenger.Default;
            enabled = false;
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
                _gdCharacter= hitInfo.transform.GetComponent<DataController>().GdCharacter;
                OnMouseOverSomething(_gdCharacter);
                _mousePosition = hitInfo.point;
            }
            
        }
        
        
        private void OnMouseOverSomething(GDCharacter gdCharacter)
        {
            if(gdCharacter==null||(_typedInteract.Equals(gdCharacter.TypedInteract)&&_preFroceAttack==_forceAttack)) return;
            _typedInteract = gdCharacter.TypedInteract;
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
        #region 订阅引用

        private ISubscription<MMouseTarget> onMouse1Walkable;
        private ISubscription<MMouseTarget> onMouse1Target;
        private ISubscription<MMouseTarget> onMouse0Target;
        private ISubscription<MMouseTarget> onMouse0Walkable;
        private ISubscription<MInput> onForceAttack;
        private ISubscription<MInput> onNormalAttack;
        private ISubscription<MInput> onStopAttack;
        private ISubscription<MMovement> onStopMove;
        private ISubscription<MMovement> onMoveTo;

        #endregion
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {

            onForceAttack = _messenger.Subscribe<MInput>(TypedInputActions.ForceAttack.ToString(), (message) =>
            {
                _forceAttack = message.ForceAttack;
            });
        }
    }
}