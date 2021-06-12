using ActionPool;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Managers;
using Scripts;
using UnityEngine;

namespace States
{
    /// <summary>
    /// 场景中的UI控制
    /// </summary>
    public class SceneUIState:BaseState
    {
        private PrefabsManager _prefabsManager;
        public SceneUIState()
        {
            _prefabsManager=PrefabsManager.Instance;
            RegistInputActions();
        }
        
        #region 订阅引用

        private ISubscription<MouseTargetMessage> onMouse1Walkable;
        private ISubscription<MouseTargetMessage> onMouse1Target;
        private ISubscription<MouseTargetMessage> onMouse0Target;
        private ISubscription<MouseTargetMessage> onMouse0Walkable;
        private ISubscription<InputMessage> onForceAttack;
        private ISubscription<InputMessage> onNormalAttack;
        private ISubscription<InputMessage> onStopAttack;
        private ISubscription<MovementMessage> onStopMove;
        private ISubscription<MovementMessage> onMoveTo;

        #endregion
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            onMouse1Walkable=_messenger.Subscribe<MouseTargetMessage>(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),
                (message) =>
                {
                    OnClickMouseRightWalkable(true, message.MousePosition);
                });
        }
        
        /// <summary>
        /// 鼠标右键点击移动平台，进行移动，移动只需提供位置即可。
        /// 当位置为null，代表按照之前的位置进行移动，不改变移动目标
        /// </summary>
        private void OnClickMouseRightWalkable(bool isNewTarget,Vector3 position)
        {
            _prefabsManager.ClickArrow.transform.position = position;
        }
        protected override void DoUpdate()
        {
            
        }
    }
}