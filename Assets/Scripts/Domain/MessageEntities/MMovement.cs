using Loxodon.Framework.Messaging;
using UnityEngine;

namespace Domain.MessageEntities
{
    public class MMovement:MessageBase
    {
        private Vector3 _targetPosition;

        public Vector3 TargetPosition
        {
            get => _targetPosition;
            set => _targetPosition = value;
        }


        public MMovement(object sender) : base(sender)
        {
        }
    }
}