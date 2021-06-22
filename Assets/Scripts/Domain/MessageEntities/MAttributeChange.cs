using Commons;
using Loxodon.Framework.Messaging;

namespace Domain.MessageEntities
{
    public class MAttributeChange:MessageBase
    {

        private TypedAttribute _typedAttribute;
        public MAttributeChange(object sender) : base(sender)
        {
        }

        public TypedAttribute TypedAttribute
        {
            get => _typedAttribute;
            set => _typedAttribute = value;
        }
    }
}