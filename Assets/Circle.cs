using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class Circle : Dot
    {
        private CircleController _circleController;
        private Animation _animation;

        private bool _canMove = false;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private bool _isPointerComing = false;
        private bool _isSelectable = true;

        private const string SpawnAnimation = "CircleSpawnAnimation";
        
        public bool IsSelectable
        {
            get { return _isSelectable; }
            set { _isSelectable = value; }
        }

        private void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        public void Initialize(CircleController circleController)
        {
            _circleController = circleController;
            _animation.Play(SpawnAnimation);
            _isSelectable = true;
        }

        private void OnSpawnAnimationCompleted()
        {
            SetTarget();
        }

        private void Start()
        {
            
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void Update()
        {
            if(!GameManager.Instance.IsLevelFailed && _canMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, _targetPosition, Time.deltaTime * GameManager.DefaultGameProperties.MoveSpeed);

                if (Vector3.Distance(transform.position,_targetPosition) <= 0.05f)
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
            if (_isPointerComing && !_isSelectable)
                return;

            GameEventCaller.Instance.OnCircleSelected(this);
        }

        public void CircleSelected()
        {
            _isSelectable = false;
            _isPointerComing = true;
        }

        public void OnPointerReached()
        {
            //print("Pointer Reached");
            _isPointerComing = false;
        }
    }
}
