using System;
using ActionPool;
using Data;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Scripts;
using Tools;
using UnityEngine;

namespace States
{

    /// <summary>
    /// 移动类型
    /// </summary>
    public enum TypedMovement
    {
        // 单纯移动到目标点，不进行任何动作
        Simple,
        // 移动后进行交互
        Interact,
    }
    /// <summary>
    /// 管理角色移动，需要接收到移动指令才能移动（点击移动）
    /// </summary>
    public class MovementState:BaseState
    {

        private Transform _transform;

        /// <summary>
        /// 不提供外部访问
        /// </summary>
        private PlayerData _player;

        private Vector3 currentVelocity;

        /// <summary>
        /// 冲刺指令
        /// </summary>
        private bool isSprinting;

        private CapsuleCollider _collider;

        private bool _forceAttack;

        private ISubscription<MMouseTarget> onMouse1Walkable;
        private ISubscription<MMouseTarget> onMouse1Target;
        private ISubscription<MMouseTarget> onMouse0Target;
        private ISubscription<MMouseTarget> onMouse0Walkable;
        private ISubscription<MInput> forceAttack;
        private ISubscription<MMovement> stopMove;
        private ISubscription<MMovement> moveTo;
        /// <summary>
        /// 引用属性必须在构造器中提前获取引用。
        /// </summary>
        /// <param name="player"></param>
        public MovementState(PlayerData player)
        {
            _player = player;
            // 获取需要属性
            _transform = player.Transform;
            _collider = player.Collider;
            player.MovementState = this;
            RegistInputActions();
            StartAction();
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            // 此处必须获取引用，不然会自动被回收
            onMouse1Walkable=_messenger.Subscribe<MMouseTarget>(
                TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),
                OnClickMouseRightWalkable
                );
            onMouse1Target = _messenger.Subscribe<MMouseTarget>(
                TypedInputActions.OnKeyDown_Mouse1_Target.ToString(), 
                OnClickMouseRightTarget
                );
            onMouse0Target = _messenger.Subscribe<MMouseTarget>(
                TypedInputActions.OnKeyDown_Mouse0_Target.ToString(),
                OnClickMouseLeftForceTarget
                );
            forceAttack = _messenger.Subscribe<MInput>(
                TypedInputActions.ForceAttack.ToString(),
                (message) =>
                {
                    _forceAttack = message.ForceAttack;
                });
           
            stopMove = _messenger.Subscribe<MMovement>( 
                TypedInputActions.StopMove.ToString(), 
                StopMove);
            
            moveTo=_messenger.Subscribe<MMovement>( 
                TypedInputActions.MoveTo.ToString(), 
                MoveTo);
        }

        public void StopMove(MMovement mMovement)
        {
            StopAction();
            if (_player.TargetMovePosition != _transform.position)
            {                
                _transform.rotation = Quaternion.LookRotation(_player.TargetMovePosition-_transform.position);
            }
            _player.IsMoving = false;
        }
        private void MoveTo(MMovement mMovement)
        {
           MoveTo(mMovement.TargetPosition);
        }
        private void MoveTo(Vector3 position)
        {
            _player.TargetMovePosition = position;
            CalculateNavmesh();
            StartAction();
        }
        /// <summary>
        /// 鼠标右键点击移动平台，进行移动，移动只需提供位置即可。
        /// 当位置为null，代表按照之前的位置进行移动，不改变移动目标
        /// </summary>
        private void OnClickMouseRightWalkable(MMouseTarget mMouseClickRight)
        {
            OnClickMouseRightWalkable(true, mMouseClickRight.MousePosition);
        }
        /// <summary>
        /// 鼠标右键点击移动平台，进行移动，移动只需提供位置即可。
        /// 当位置为null，代表按照之前的位置进行移动，不改变移动目标
        /// </summary>
        private void OnClickMouseRightWalkable(bool isNewTarget,Vector3 position)
        {
            // 鼠标右键点击地面进行移动会打断攻击动作
            _messenger.Publish<MInput>(TypedInputActions.StopAttack.ToString(),null);
            var position1 = _transform.position;
            float inputHorizontal = position.x>position1.x?1:-1;
            float inputVertical = ((position.z - position1.z) / (position.x - position1.x)) * inputHorizontal;
            _player.InputVector = CameraRelativeInput(inputHorizontal, inputVertical);
            if (isNewTarget)
            {
                _player.TargetMovePosition = position;
                CalculateNavmesh();
                StartAction();
            }
        }
        /// <summary>
        /// 鼠标右键点击移动平台，进行移动，移动只需提供位置即可。
        /// 当位置为null，代表按照之前的位置进行移动，不改变移动目标
        /// </summary>
        private void OnClickMouseRightTarget(MMouseTarget clickRight)
        {
            
        }
        /// <summary>
        /// 鼠标右键选择目标，前往互动或者前往攻击，此处仅进行移动。
        /// </summary>
        private void OnClickMouseLeftForceTarget(MMouseTarget clickLeft)
        {
            GameData gameData = clickLeft.GameData;
            if(!_forceAttack) return;
            // 点击自己直接退出
            if(_player.Uid.Equals(gameData.Uid)) return;
            _player.TargetMovePosition = gameData.Transform.position;
            if ((_player.TargetMovePosition - _transform.position).magnitude < _player.AttackRange)
            {
                _transform.rotation = Quaternion.LookRotation(_player.TargetMovePosition-_transform.position);
                return;
            }
            // 首先计算路径
            CalculateNavmesh();
            StartAction();
        }
        /// <summary>
        /// 鼠标右键选择目标，前往互动或者前往攻击，此处仅进行移动。
        /// </summary>
        private void OnClickMouseLeftForceTarget(GameData gameData)
        {
            if(!_forceAttack) return;
            // 点击自己直接退出
            if(_player.Uid.Equals(gameData.Uid)) return;
            
            _player.TargetMovePosition = gameData.Transform.position;
            if ((_player.TargetMovePosition - _transform.position).magnitude < _player.AttackRange)
            {
                _transform.rotation = Quaternion.LookRotation(_player.TargetMovePosition-_transform.position);
                return;
            }
            // 首先计算路径
            CalculateNavmesh();
            StartAction();
        }
        public void CalculateNavmesh()
        {
            if (!calculating_path)
            {
                calculating_path = true;
                path_found = false;
                path_index = 0;
                auto_move_target_next = _player.TargetMovePosition; //Default
                NavMeshTool.CalculatePath(_transform.position, _player.TargetMovePosition, 1 << 0, FinishCalculateNavmesh);
            }
        }

        private void FinishCalculateNavmesh(NavMeshToolPath path)
        {
            calculating_path = false;
            path_found = path.success;
            nav_paths = path.path;
            path_index = 0;
        }
        /// <summary>
        /// Movement based off camera facing.
        /// </summary>
        private Vector3 CameraRelativeInput(float inputHorizontal, float inputVertical)
        {
            //Forward vector relative to the camera along the x-z plane.
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;
            //Right vector relative to the camera always orthogonal to the forward vector.
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            Vector3 relativeVelocity = inputHorizontal * right + inputVertical * forward;
            //Reduce input for diagonal movement.
            if(relativeVelocity.magnitude > 1)
            {
                relativeVelocity.Normalize();
            }
            return relativeVelocity;
        }
        protected override void DoUpdate()
        {
            if (CheckMovable())
            {
                // 状态更新后，通过当前速度每帧移动角色
                Move();
                // 地面检测
                //DetectGrounded();
            }
            else
            {
                StopAction();
                _player.IsMoving = false;
            }
        }
        
        /// <summary>
        /// 判断角色是否能够移动
        /// </summary>
        /// <returns></returns>
        private bool CheckMovable()
        {
            return CheckMovable_CanMove()&&CheckMovable_IsArrived();
        }
        /// <summary>
        /// 判断角色是否已经到达目标位置。
        /// </summary>
        /// <returns></returns>
        private bool CheckMovable_IsArrived()
        {
            bool isArrive = (_transform.position - _player.TargetMovePosition).magnitude > .15f;

            if (!isArrive)
            {
                _transform.position = _player.TargetMovePosition;
            }
            return isArrive;
        }

        /// <summary>
        /// 检测角色是否能够移动（被敌人技能控制、死亡等）
        /// </summary>
        /// <returns></returns>
        private bool CheckMovable_CanMove()
        {
            return true;
        }
        
        /// <summary>
        /// 当前角色与移动目标点的距离。
        /// </summary>
        /// <returns></returns>
        private float MoveTargetDistance()
        {
            
            double dis = Math.Sqrt(Math.Pow(_player.TargetMovePosition.x - _transform.position.x, 2) + Math.Pow(_player.TargetMovePosition.z - _transform.position.z, 2));
            return (float) dis;
        }

        #region 寻路相关
        private int path_index;
        private Vector3[] nav_paths = new Vector3[0];
        private bool calculating_path = false;
        private bool path_found = false;
        private bool use_navmesh = false;
        
        private bool simple_move = false;
        private Vector3 auto_move_target_next;
        private float auto_move_timer = 0f;
        private bool is_grounded;
        public LayerMask ground_layer = ~0;
        private Vector3 ground_normal = Vector3.up;
        public float slope_angle_max = 45f;
        private Vector3 move;
        private Vector3 facing;
        private bool across;



        #endregion
        #region Move()方法全局缓存属性（减小栈帧大小（或许））
        private Vector3 voidPosition;
        private Vector3 move_dir_next;
        private Vector3 tmove;
        private Vector3 move_dir_total;
        private Vector3 move_dir;
        private float moveSpeed;
        private bool isSpeedChange;
        private Vector3 curPosition;
        private float predict_distance;
        private float pointDistance;
        #endregion
        // 进行移动
        private void Move()
        {
            // 没有寻找到路径或者路径中只有一个点，退出（默认路径中有两个点，起点和终点）
            if(!path_found) return;
            _player.IsMoving = true;
            across = false;
            curPosition=_transform.position;
            voidPosition= curPosition;
            if (Math.Abs(moveSpeed - GetMoveSpeed()) > .01f)
            {
                isSpeedChange = true;
                moveSpeed = GetMoveSpeed();
            }
            // 预测这次移动的距离
            predict_distance =moveSpeed * Time.fixedDeltaTime;
            float pointDistance = 0f;
            while (path_index  < nav_paths.Length)
            {
                auto_move_target_next = nav_paths[path_index];
                var position = curPosition;
                move_dir_next = auto_move_target_next - position;
                move_dir_next.y = 0f;
                pointDistance = move_dir_next.magnitude;
                // 这两点之间的距离小于预测值
                if ( pointDistance<= predict_distance)
                {
                    voidPosition = auto_move_target_next;
                    path_index++;
                    // 越过终点，取消溢出距离，速度设置为0
                    if (path_index >= nav_paths.Length)
                    {
                        predict_distance = 0;
                    }
                    // 未越过终点，距离减少
                    else
                    {
                        predict_distance -= pointDistance;
                    }
                    // 标记是否越过
                    across = true;
                }
                // 两点之间的距离大于预测值，可以直接移动，退出
                else
                {
                    break;
                }
            }
            // 如果有越过情况发生，直接赋值位置并设置方向，（角色的速度太快，导致每帧刷新的位置太远，因此，直接每帧进行赋值，越过目标点）。
            if (across)
            {
                voidPosition+=move_dir_next.normalized* predict_distance;
                _transform.position = voidPosition;
                // 直接设置朝向
                _transform.rotation = Quaternion.LookRotation(move_dir_next);
            }
            // 正常速度移动+转向
            else
            {
                tmove = move_dir_next.normalized * moveSpeed*Time.fixedDeltaTime;
                _transform.position += tmove;
                facing = new Vector3(tmove.x, 0f, tmove.z).normalized;
                RotateTowardsMovementDir();
            }
        }

        private void DetectGrounded()
        {
            float hradius = GetColliderHeightRadius();
            float radius = GetColliderRadius();
            Vector3 center = GetColliderCenter();

            float gdist; Vector3 gnormal;
            is_grounded = PhysicsTool.DetectGround(_transform, center, hradius, radius, ground_layer, out gdist, out gnormal);
            ground_normal = gnormal;

            float slope_angle = Vector3.Angle(ground_normal, Vector3.up);
            is_grounded = is_grounded && slope_angle <= slope_angle_max;
        }
        public float GetMoveSpeed()
        {
            float base_speed = _player.MoveSpeed;
            return base_speed ;
        }
        public float GetColliderHeightRadius()
        {
            Vector3 scale = _transform.lossyScale;
            return _collider.height* scale.y * 0.5f + .3f; //radius is half the height minus offset
        }
        public Vector3 GetColliderCenter()
        {
            Vector3 scale = _transform.lossyScale;
            return _collider.transform.position + Vector3.Scale(_collider.center, scale);
        }
        public float GetColliderRadius()
        {
            Vector3 scale = _transform.lossyScale;
            return _collider.radius * (scale.x + scale.y) * 0.5f;
        }
        /// <summary>
        /// 朝向移动方向
        /// </summary>
        public void RotateTowardsMovementDir()
        {
            _transform.rotation=Quaternion.RotateTowards(_transform.rotation, Quaternion.LookRotation(facing, Vector3.up), _player.RotateSpeed * Time.fixedDeltaTime);
        }
        /// <summary>
        /// 旋转向目标
        /// </summary>
        /// <param name="targetPosition"></param>
        public void RotateTowardsTarget(Vector3 targetPosition)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - new Vector3(_transform.position.x, 0, _transform.position.z));
            _transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(_transform.eulerAngles.y, targetRotation.eulerAngles.y, _player.RotateSpeed * Time.deltaTime);
        }
        
        
    }
}