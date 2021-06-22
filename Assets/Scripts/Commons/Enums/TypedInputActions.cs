namespace Scripts
{
    public enum TypedInputActions
    {
        /// <summary>
        /// 鼠标左键点击地面
        /// </summary>
        OnKeyDown_Mouse0_Walkable,
        /// <summary>
        /// 鼠标右键点击地面
        /// </summary>
        OnKeyDown_Mouse1_Walkable,

        /// <summary>
        /// 鼠标左键点击对象
        /// </summary>
        OnKeyDown_Mouse0_Target,
        /// <summary>
        /// 鼠标右键点击对象
        /// </summary>
        OnKeyDown_Mouse1_Target,
        
        /// <summary>
        /// 鼠标移动到对象上
        /// </summary>
        OnMouseOver,
        /// <summary>
        /// 强制攻击命令（alt+攻击目标）
        /// </summary>
        OnForceAttack,
        /// <summary>
        /// 强制攻击命令取消（alt+攻击目标）
        /// </summary>
        ForceAttack,
        StopAttack,
        
        
        /// <summary>
        /// 前往指定地点
        /// </summary>
        MoveTo,
        /// <summary>
        /// 停止移动
        /// </summary>
        StopMove,
        
        
        /// <summary>
        /// 普通攻击
        /// </summary>
        AnimNormalAttack,
        AnimNormalAttackStop,
        NormalAttack,
        
        /// <summary>
        /// 当前操控角色
        /// </summary>
        CurrentControlledPlayer
        
    }
}