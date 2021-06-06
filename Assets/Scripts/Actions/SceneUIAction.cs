using ActionPool;
using Managers;
using Scripts;
using UnityEngine;

namespace Actions
{
    public class SceneUIAction:BaseAction
    {
        private PrefabsManager _prefabsManager;
        public SceneUIAction()
        {
            _prefabsManager=PrefabsManager.Instance;
            RegistInputActions();
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            EventCenter.AddListener<bool,Vector3>(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),OnClickMouseRightWalkable);
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