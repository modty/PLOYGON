using Loxodon.Framework.Messaging;

namespace Domain.MessageEntities
{
    public class MInput:MessageBase
    {
        private bool _forceAttack;

        public bool ForceAttack
        {
            get => _forceAttack;
            set => _forceAttack = value;
        }

        private int _mormalAttack;

        public int MormalAttack
        {
            get => _mormalAttack;
            set => _mormalAttack = value;
        }

        public MInput(object sender) : base(sender)
        {
            
        }
    }
}