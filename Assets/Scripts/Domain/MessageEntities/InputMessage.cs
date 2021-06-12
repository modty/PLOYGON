using Loxodon.Framework.Messaging;

namespace Domain.MessageEntities
{
    public class InputMessage:MessageBase
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

        public InputMessage(object sender) : base(sender)
        {
            
        }
    }
}