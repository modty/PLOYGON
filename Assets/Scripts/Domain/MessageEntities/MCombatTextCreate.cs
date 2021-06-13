using Loxodon.Framework.Messaging;
using Managers;
using UnityEngine;

namespace Domain.MessageEntities
{
    public class MCombatTextCreate:MessageBase
    {
        private Vector3 _position;
        private string _text;
        private SCTTYPE _type;
        private bool _crit;
        private bool _direction;
        public MCombatTextCreate(object sender) : base(sender)
        {
        }

        public MCombatTextCreate(object sender, Vector3 position, string text, SCTTYPE type, bool crit) : base(sender)
        {
            _position = position;
            _text = text;
            _type = type;
            _crit = crit;
        }

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public SCTTYPE Type
        {
            get => _type;
            set => _type = value;
        }

        public bool Crit
        {
            get => _crit;
            set => _crit = value;
        }

        public bool Direction
        {
            get => _direction;
            set => _direction = value;
        }
    }
}