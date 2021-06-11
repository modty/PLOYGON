using ActionPool;
using Commons;
using Data;
using Scripts;
using States;
using UnityEngine;
using AnimationState = States.AnimationState;

namespace Controller
{
    /// <summary>
    /// 用户输入接收
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        /// <summary>
        /// 临时链接
        /// </summary>
        public GameObject modelObject;

        public PlayerAttribute _attribute;
        private MovementState _movementState;
        private AnimationState _animationState;
        private SceneUIState _sceneUIState;
        private NormalAttackState _normalAttackState;
        private SkillAreaState _skillAreaState;
        public float moveSpeed=5f;
        public AnimEventController eventController;
        private MouseController _mouse;



        // Start is called before the first frame update
        void Start()
        {
            Debug.Log(_attribute.Uid);
            _movementState = new MovementState(_attribute);
            _animationState = new AnimationState(_attribute);
            _sceneUIState = new SceneUIState();
            _normalAttackState = new NormalAttackState(_attribute);
            _skillAreaState = new SkillAreaState(_attribute);
            _mouse=MouseController.Get();
            EventCenter.Broadcast("UIElement:"+TypedUIElements.PlayerMes,(GameData)_attribute);
        }

        // Update is called once per frame
        void Update()
        {
            _attribute.MoveSpeed = moveSpeed;
            _animationState.Update();
            _skillAreaState.Update();

            if (Input.GetKeyDown(KeyCode.D))
            {
                _attribute.Health.UpdateCurrentValue(-1000);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                _attribute.Health.UpdateCurrentValue(1000);
            }
        
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                GameData gameData = _mouse.GameData;
                if (gameData != null)
                {
                    // 鼠标点击到（可移动位置）
                    if (gameData.CanMoved&&_mouse.MousePosition!=Vector3.zero)
                    {
                        EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),
                            true,_mouse.MousePosition);
                    }
                    else
                    {
                        EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse1_Target.ToString(), gameData);
                    }

                    // 地面暂时不加入点击选择
                    if (!(gameData is FloorAttribute))
                    {
                        _attribute.Target = gameData;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameData gameData = _mouse.GameData;
                if (gameData != null)
                {
                    // 鼠标点击到（可移动位置）
                    if (gameData.CanMoved&&_mouse.MousePosition!=Vector3.zero)
                    {
                        EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse0_Walkable.ToString(), gameData);
                    }
                    else
                    {
                        EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse0_Target.ToString(), gameData);
                    }

                    // 地面暂时不加入点击选择
                    if (!(gameData is FloorAttribute))
                    {
                        _attribute.Target = gameData;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EventCenter.Broadcast(TypedInputActions.OnForceAttack.ToString());
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                EventCenter.Broadcast(TypedInputActions.OffForceAttack.ToString());
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),6);
            }
        
        }

        private void FixedUpdate()
        {
            _normalAttackState.Update();
            _movementState.Update();
        }
    }
}
