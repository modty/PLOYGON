using UnityEngine;

namespace Scripts.Commons
{
    public class Constants_Anim
    {
        public static readonly int Moving_Bool = Animator.StringToHash("Moving");
        public static readonly int Strafing_Bool = Animator.StringToHash("Strafing");
        public static readonly int Aiming_Bool = Animator.StringToHash("Aiming");
        public static readonly int Stunned_Bool = Animator.StringToHash("Stunned");
        public static readonly int Shielding_Bool = Animator.StringToHash("Shielding");
        public static readonly int Swimming_Bool = Animator.StringToHash("Swimming");
        public static readonly int Blocking_Bool = Animator.StringToHash("Blocking");
        public static readonly int Injured_Bool = Animator.StringToHash("Injured");
        public static readonly int Crouch_Bool = Animator.StringToHash("Crouch");
        public static readonly int Sprint_Bool = Animator.StringToHash("Sprint");
        
        public static readonly int AnimationSpeed_Float = Animator.StringToHash("AnimationSpeed");
        public static readonly int VelocityX_Float = Animator.StringToHash("Velocity X");
        public static readonly int VelocityZ_Float = Animator.StringToHash("Velocity Z");
        public static readonly int AimHorizontal_Float = Animator.StringToHash("AimHorizontal");
        public static readonly int AimVertical_Float = Animator.StringToHash("AimVertical");
        public static readonly int BowPull_Float = Animator.StringToHash("BowPull");
        public static readonly int Charge_Float = Animator.StringToHash("Charge");
        public static readonly int NormalAttackSpeed_Float = Animator.StringToHash("NormalAttackSpeed");

        public static readonly int Weapon_Int = Animator.StringToHash("Weapon");
        public static readonly int WeaponSwitch_Int = Animator.StringToHash("WeaponSwitch");
        public static readonly int LeftRight_Int = Animator.StringToHash("LeftRight");
        public static readonly int LeftWeapon_Int = Animator.StringToHash("LeftWeapon");
        public static readonly int RightWeapon_Int = Animator.StringToHash("RightWeapon");
        public static readonly int AttackSide_Int = Animator.StringToHash("AttackSide");
        public static readonly int Jumping_Int = Animator.StringToHash("Jumping");
        public static readonly int SheathLocation_Int = Animator.StringToHash("SheathLocation");
        public static readonly int Talking_Int = Animator.StringToHash("Talking");
        public static readonly int Action_Int = Animator.StringToHash("Action");
        
        public static readonly int IdleTrigger = Animator.StringToHash("IdleTrigger");
        public static readonly int ActionTrigger = Animator.StringToHash("ActionTrigger");
        public static readonly int ClimbLadderTrigger = Animator.StringToHash("ClimbLadderTrigger");
        public static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        public static readonly int AttackKickTrigger = Animator.StringToHash("AttackKickTrigger");
        public static readonly int AttackDualTrigger = Animator.StringToHash("AttackDualTrigger");
        public static readonly int AttackCastTrigger = Animator.StringToHash("AttackCastTrigger");
        public static readonly int SpecialAttackTrigger = Animator.StringToHash("SpecialAttackTrigger");
        public static readonly int SpecialEndTrigger = Animator.StringToHash("SpecialEndTrigger");
        public static readonly int CastTrigger = Animator.StringToHash("CastTrigger");
        public static readonly int CastEndTrigger = Animator.StringToHash("CastEndTrigger");
        public static readonly int GetHitTrigger = Animator.StringToHash("GetHitTrigger");
        public static readonly int RollTrigger = Animator.StringToHash("RollTrigger");
        public static readonly int TurnLeftTrigger = Animator.StringToHash("TurnLeftTrigger");
        public static readonly int TurnRightTrigger = Animator.StringToHash("TurnRightTrigger");
        public static readonly int WeaponSheathTrigger = Animator.StringToHash("WeaponSheathTrigger");
        public static readonly int WeaponUnsheathTrigger = Animator.StringToHash("WeaponUnsheathTrigger");
        public static readonly int DodgeTrigger = Animator.StringToHash("DodgeTrigger");
        public static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
        public static readonly int BlockTrigger = Animator.StringToHash("BlockTrigger");
        public static readonly int Death1Trigger = Animator.StringToHash("Death1Trigger");
        public static readonly int Revive1Trigger = Animator.StringToHash("Revive1Trigger");
        public static readonly int BlockBreakTrigger = Animator.StringToHash("BlockBreakTrigger");
        public static readonly int SwimTrigger = Animator.StringToHash("SwimTrigger");
        public static readonly int ReloadTrigger = Animator.StringToHash("ReloadTrigger");
        public static readonly int InstantSwitchTrigger = Animator.StringToHash("InstantSwitchTrigger");
        
        
        
       
    }
}