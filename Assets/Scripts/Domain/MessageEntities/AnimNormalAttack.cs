using Loxodon.Framework.Messaging;

namespace Domain.MessageEntities
{
    public class AnimNormalAttack:MessageBase
    {
        private int _weaponType;
        private int _action;
        public AnimNormalAttack(object sender) : base(sender)
        {
        }

        public int WeaponType
        {
            get => _weaponType;
            set => _weaponType = value;
        }

        public int Action
        {
            get => _action;
            set => _action = value;
        }
    }
}