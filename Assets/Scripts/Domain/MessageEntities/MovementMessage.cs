using Loxodon.Framework.Messaging;
using UnityEngine;

namespace Domain.MessageEntities
{
    public class MovementMessage:MessageBase
    {
        private Vector3 _targetPosition;

        public Vector3 TargetPosition
        {
            get => _targetPosition;
            set => _targetPosition = value;
        }


        public MovementMessage(object sender) : base(sender)
        {
        }
    }
}