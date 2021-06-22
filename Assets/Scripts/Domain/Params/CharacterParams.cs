using Commons;
using UnityEngine;

namespace UnityTemplateProjects.Domain.Params
{
    public class CharacterParams
    {
        private bool _isPlayer;
        private float _rotateSpeed;
        private float _attackRange;
        private float _moveSpeed;

        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        private float _moveAcceleration;
        private TypedWeapon _weaponType;
        private TypedInteract _interactType;
        private Vector3 _position;

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public TypedInteract InteractType
        {
            get => _interactType;
            set => _interactType = value;
        }

        private int _health;
        private int _attackDamage;
        private float _attackSpeed;

        public bool IsPlayer
        {
            get => _isPlayer;
            set => _isPlayer = value;
        }

        public float RotateSpeed
        {
            get => _rotateSpeed;
            set => _rotateSpeed = value;
        }

        public float AttackRange
        {
            get => _attackRange;
            set => _attackRange = value;
        }

        public float MoveAcceleration
        {
            get => _moveAcceleration;
            set => _moveAcceleration = value;
        }

        public TypedWeapon WeaponType
        {
            get => _weaponType;
            set => _weaponType = value;
        }

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public int AttackDamage
        {
            get => _attackDamage;
            set => _attackDamage = value;
        }

        public float AttackSpeed
        {
            get => _attackSpeed;
            set => _attackSpeed = value;
        }
    }
}