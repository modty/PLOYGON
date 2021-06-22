using Loxodon.Framework.Messaging;

namespace Domain.MessageEntities
{
    public class MAnimNormalAttack:MessageBase
    {
        private int _weaponType;
        private int _action;
        private bool _stop;

        public MAnimNormalAttack(object sender, bool stop) : base(sender)
        {
            _stop = stop;
        }

        public bool Stop
        {
            get => _stop;
            set => _stop = value;
        }

        public MAnimNormalAttack(object sender) : base(sender)
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