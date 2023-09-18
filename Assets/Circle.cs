using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class Circle : Dot
    {
        private CircleController _circleController;
        private SpriteRenderer _spriteRenderer;

        private bool _canMove = false;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private bool _isSelectable = true;
        private float _moveSpeed = 0;
        private float _maxTravelTime = 0;
        private float _travelTimer = 0;

        private const string SpawnAnimation = "CircleSpawnAnimation";
        
        public bool IsSelectable
        {
            get { return _isSelectable; }
            set { _isSelectable = value; }
        }

        private void Awake()
        {
            base.Awake();
        }

        public void Initialize(CircleController circleController,float moveSpeed,float maxTravelTime)
        {
            _circleController = circleController;
            _animation.Play(SpawnAnimation);
            _isSelectable = true;
            SetMoveSpeed(moveSpeed);
            SetMaxTravelTime(maxTravelTime);
        }

        private void OnSpawnAnimationCompleted()
        {
            SetTarget();
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetMoveSpeed(float newSpeed)
        {
            _moveSpeed = newSpeed;
        }

        private void Update()
        {
            if(!GameManager.Instance.IsLevelFailed && _canMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _moveSpeed);
                _travelTimer += Time.deltaTime;

                if (Vector3.Distance(transform.position,_targetPosition) <= 0.05f || _travelTimer >= _maxTravelTime)
                    OnTargetReached();
            }
        }

        private void SetTarget()
        {
            _startPosition = transform.position;
            _targetPosition = _circleController.GetRandomPosition();
            _canMove = true;
        }

        private void OnTargetReached()
        {
            _travelTimer = 0;
            _canMove = false;
            Invoke(nameof(SetTarget), GameManager.DefaultGameProperties.WaitTime);
        }

        private void OnMouseEnter()
        {
            if (!GameManager.Instance.IsLevelFailed && InputManager.IsSwiping)
                TrySelect();
        }

        private void OnMouseDown()
        {
            if (!GameManager.Instance.IsLevelFailed)
                TrySelect();
        }

        private void TrySelect()
        {
            if (!_isSelectable)
                return;

            GameEventCaller.Instance.OnCircleSelected(this);
        }

        public void CircleSelected()
        {
            _isSelectable = false;
        }

        public void SetMaxTravelTime(float travelTime)
        {
            //Random olsunlar
            _maxTravelTime = UnityEngine.Random.Range(travelTime - 1, travelTime + 1);
        }

        public void OnPointerReached()
        {
            //print("Pointer Reached");
        }

        public void MakeShatter()
        {
            ParticleSystem particleSystem = ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.circleShatter, null, transform.position, false);
            particleSystem.name = name;
            Destroy(gameObject);
            //_spriteRenderer.enabled = false;
        }
    }
}
